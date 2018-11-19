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
        private UserManager<AppUser> _userManager;

        public TasksService
            (IRepository<Tasks> tasksRepo,
            UserManager<AppUser> userManager)
        {
            _tasksRepo = tasksRepo;
            _userManager = userManager;
        }

        public async Task<OperationResult<TaskDTO>> GetTasks(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var data = GetTasksData(user.Id, user.IsChild);

                return new OperationResult<TaskDTO>()
                {
                    Success = true,
                    Messages = new[] { "You have following lists" },
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

            return new OperationResult<TaskDTO>()
            {
                Messages = new[] { "Invalid token" }
            };
        }

        public async Task<OperationResult<TaskDTO>> GetLatestTask(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
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
                    Messages = new[] { "You have following lists" },
                    Data = new []{ new TaskDTO()
                    {
                        Id = task.Id,
                        Name = task.Name,
                        StartDate = task.StartDate,
                        EndDate = task.EndDate,
                        IsFinished = task.IsFinished
                    }}
                };
            }

            return new OperationResult<TaskDTO>()
            {
                Messages = new[] { "Invalid token" }
            };
        }

        public async Task<OperationResult<Tasks>> AddTasks(string userId, TaskToManipulateDTO[] items)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationResult<Tasks>> EditTask(string userId, string taskId, TaskToManipulateDTO item)
        {
            throw new NotImplementedException();
        }

        public OperationResult RemoveTasks(string userId, string[] taskIds)
        {
            throw new NotImplementedException();
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
    }
}
