using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nocrastination.Core.Entities;
using Nocrastination.Data;
using Nocrastination.Interfaces;
using Nocrastination.Models;
using Nocrastination.Models.DTO;

namespace Nocrastination.Services
{
    public class TasksService : ITasksService
    {
        private IRepository<Tasks> _tasksRepo;
        private IUserService _userSrv;

        public TasksService
            (IRepository<Tasks> tasksRepo,
            IUserService userSrv)
        {
            _tasksRepo = tasksRepo;
            _userSrv = userSrv;
        }

        public async Task<OperationResult<TaskDTO>> GetTasks(string userId)
        {
            var user = await _userSrv.FindUserById(userId);

            var data = GetTasksData(user.Id, user.IsChild);

            return new OperationResult<TaskDTO>()
            {
                Success = true,
                Messages = new[] { "You have following lists." },
                Data = data.Select(x => new TaskDTO()
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsFinished = x.IsFinished
                })
            };
        }

        public async Task<OperationResult<TaskDTO>> GetLatestTask(string userId)
        {
            var user = await _userSrv.FindUserById(userId);

            var now = DateTime.UtcNow;

            var data = GetTasksData(user.Id, user.IsChild).OrderBy(x => x.EndDate);

            var task = data.FirstOrDefault(x => x.StartDate <= now &&
                                                x.EndDate >= now);
            if (task == null)
            {
                task = data.FirstOrDefault(x => x.EndDate >= now);
            }

            return new OperationResult<TaskDTO>()
            {
                Success = true,
                Messages = new[] { "You have following lists." },
                Data = new[]{ new TaskDTO()
                {
                    Id = task.Id,
                    Name = task.Name,
                    StartDate = task.StartDate,
                    EndDate = task.EndDate,
                    IsFinished = task.IsFinished
                }}
            };
        }

        public OperationResult<Tasks> AddTasks(string userId, TaskToManipulateDTO[] items)
        {
            var childUser = _userSrv.FindChildByParentId(userId);

            if (childUser != null)
            {
                var tasks = new List<Tasks>() { };

                var messages = new List<string>() { };

                foreach (var item in items)
                {
                    if (!IsValid(item))
                    {
                        messages.Add($"Task with name = [{item.Name}], " +
                                     $"start_time = [{item.StartDate}] and " +
                                     $"end_time = [{item.EndDate}] " +
                                     "may not be added cause of ovelapping.");
                        continue;
                    }

                    tasks.Add(new Tasks()
                    {
                        Name = item.Name,
                        StartDate = (DateTime)item.StartDate,
                        EndDate = (DateTime)item.EndDate,
                        ParentId = userId,
                        ChildId = childUser.Id
                    });
                }

                return new OperationResult<Tasks>()
                {
                    Success = true,
                    Messages = new[] { "Following tasks were added successfully." },
                    Data = _tasksRepo.Add(tasks)
                };
            }

            return new OperationResult<Tasks>()
            {
                Messages = new[] { "You don`t have registered children." }
            };
        }

        public OperationResult<Tasks> EditTask(string userId, string taskId, TaskToManipulateDTO item)
        {
            if (!IsTaskExists(taskId, out var task))
            {
                return new OperationResult<Tasks>()
                {
                    Messages = new[] { $"There is no task with id = [{taskId}]" }
                };
            }

            if (task.ParentId != userId)
            {
                return new OperationResult<Tasks>()
                {
                    Messages = new[] { "You have no right to do this" }
                };
            }

            task.Name = item.Name ?? task.Name;
            task.IsFinished = item.IsFinished ?? task.IsFinished;
            task.StartDate = item.StartDate ?? task.StartDate;
            task.EndDate = item.EndDate ?? task.EndDate;

            return new OperationResult<Tasks>()
            {
                Success = true,
                Messages = new[] { "Task was updated successfully." },
                Data = new[] { _tasksRepo.Update(task) }
            };
        }

        public OperationResult RemoveTasks(string userId, string[] taskIds)
        {
            var messages = new List<string>();

            foreach (var taskId in taskIds)
            {
                if (!IsTaskExists(taskId, out var task))
                {
                    messages.Add($"There is no task with id = [{taskId}]");
                }

                if (task.ParentId != userId)
                {
                    messages.Add($"You have no right to remove task with id = [{taskId}]");
                }

                _tasksRepo.Delete(task);

                messages.Add($"Task with id = [{taskId}] was removed successfully.");
            }

            return new OperationResult()
            {
                Success = true,
                Messages = messages
            };
        }

        private bool IsValid(TaskToManipulateDTO item)
        {
            if (item.StartDate >= item.EndDate)
            {
                return false;
            }

            var crossedItem = _tasksRepo.Get(x => !(item.StartDate < x.EndDate &&
                                                    item.EndDate > x.StartDate));

            if (crossedItem != null)
            {
                return false;
            }

            return true;
        }

        private IQueryable<Tasks> GetTasksData(string userId, bool isChild)
        {
            var data = new Tasks[] { }.AsQueryable();

            if (isChild)
            {
                data = _tasksRepo.Data.Where(x => x.ChildId == userId);
            }
            else
            {
                data = _tasksRepo.Data.Where(x => x.ParentId == userId);
            }

            return data;
        }

        private bool IsTaskExists(string taskId, out Tasks task)
        {
            task = new Tasks();

            var result = Guid.TryParse(taskId, out var item);

            if (result)
            {
                task = _tasksRepo.Data.FirstOrDefault(x => x.Id == item);
                return true;
            }

            return false;
        }
    }
}
