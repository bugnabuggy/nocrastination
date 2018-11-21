using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.Entities;
using Nocrastination.Models;
using Nocrastination.Models.DTO;

namespace Nocrastination.Interfaces
{
    public interface ITasksService
    {
        Task<OperationResult<TaskDTO>> GetTasks(string userId);
        Task<OperationResult<TaskDTO>> GetLatestTask(string userId);
        OperationResult<Tasks> AddTasks(string userId, TaskToManipulateDTO item);
        OperationResult<Tasks> EditTask(string userId, string taskId, TaskToManipulateDTO item);
        OperationResult RemoveTask(string userId, string taskId);
    }
}
