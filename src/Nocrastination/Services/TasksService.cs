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
        private IRepository<ChildTask> _tasksRepo;
        private IUserService _userSrv;

        public TasksService
            (IRepository<ChildTask> tasksRepo,
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

	        List<TaskDTO> tasks = new List<TaskDTO>();

			if (task != null)
			{
				tasks.Add(new TaskDTO()
				{
					Id = task.Id,
					Name = task.Name,
					StartDate = task.StartDate,
					EndDate = task.EndDate,
					IsFinished = task.IsFinished
				});
			}

            return new OperationResult<TaskDTO>()
            {
                Success = true,
                Messages = new[] { "You have following lists." },
                Data = tasks
			};
        }

        public OperationResult<ChildTask> AddTasks(string userId, TaskToManipulateDTO item)
        {
            var childUser = _userSrv.FindChildByParentId(userId);
	        var success = true;

            if (childUser != null)
            {
                var tasks = new List<ChildTask>() { };

                var messages = new List<string>() { };

	            if (!IsValid(item, userId))
	            {
		            messages.Add($"Task with name = [{item.Name}], " +
		                         $"start_time = [{item.StartDate}] and " +
		                         $"end_time = [{item.EndDate}] " +
		                         "may not be added cause of ovelapping.");
		            success = false;

	            } else {
		            tasks.Add(new ChildTask()
		            {
			            Name = item.Name,
			            StartDate = (DateTime)item.StartDate,
			            EndDate = (DateTime)item.EndDate,
			            ParentId = userId,
			            ChildId = childUser.Id
		            });
				}

				return new OperationResult<ChildTask>()
				{
					Success = success,
					Messages = new[] { "Following tasks were added successfully." },
					Data = _tasksRepo.Add(tasks)
				};
			}

			return new OperationResult<ChildTask>()
            {
                Messages = new[] { "You don`t have registered children." }
            };
        }

        public OperationResult<ChildTask> EditTask(string userId, string taskId, TaskToManipulateDTO item)
        {
            if (!IsTaskExists(taskId, out var task))
            {
                return new OperationResult<ChildTask>()
                {
                    Messages = new[] { $"There is no childTask with id = [{taskId}]" }
                };
            }

            if (task.ParentId != userId)
            {
                return new OperationResult<ChildTask>()
                {
                    Messages = new[] { "You have no right to do this" }
                };
            }

            task.Name = item.Name ?? task.Name;
            task.IsFinished = item.IsFinished ?? task.IsFinished;
            task.StartDate = item.StartDate ?? task.StartDate;
            task.EndDate = item.EndDate ?? task.EndDate;

            return new OperationResult<ChildTask>()
            {
                Success = true,
                Messages = new[] { "Task was updated successfully." },
                Data = new[] { _tasksRepo.Update(task) }
            };
        }

        public OperationResult RemoveTask(string userId, string taskId)
        {
            var messages = new List<string>();

	        if (!IsTaskExists(taskId, out var task))
	        {
		        messages.Add($"There is no childTask with id = [{taskId}]");
	        }

	        if (task.ParentId != userId)
	        {
		        messages.Add($"You have no right to remove childTask with id = [{taskId}]");
	        }

	        _tasksRepo.Delete(task);

	        messages.Add($"Task with id = [{taskId}] was removed successfully.");

            return new OperationResult()
            {
                Success = true,
                Messages = messages
            };
        }

        private bool IsValid(TaskToManipulateDTO item, string parentId)
        {
            if (item.StartDate >= item.EndDate)
            {
                return false;
            }

            var hasCrossItems = _tasksRepo.Data.Any(x => (item.StartDate < x.EndDate &&
                                                    item.EndDate > x.StartDate) && x.ParentId.Equals(parentId));

            if (hasCrossItems)
            {
                return false;
            }

            return true;
        }

        private IQueryable<ChildTask> GetTasksData(string userId, bool isChild)
        {
            var data = new ChildTask[] { }.AsQueryable();

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

        private bool IsTaskExists(string taskId, out ChildTask childTask)
        {
            childTask = new ChildTask();

            var result = Guid.TryParse(taskId, out var item);

            if (result)
            {
                childTask = _tasksRepo.Data.FirstOrDefault(x => x.Id == item);
                return true;
            }

            return false;
        }
    }
}
