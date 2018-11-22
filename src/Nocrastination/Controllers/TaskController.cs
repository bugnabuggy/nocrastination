using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Nocrastination.Helpers;
using Nocrastination.Interfaces;
using Nocrastination.Models.DTO;
using Nocrastination.Settings;

namespace Nocrastination.Controllers
{
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : Controller
    {
        private ITasksService _taskSrv;
        private IClaimsHelper _helper;

        public TaskController
            (ITasksService taskSrv,
            IClaimsHelper helper)
        {
            _taskSrv = taskSrv;
            _helper = helper;
        }

        // GET api/tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _taskSrv.GetTasks(_helper.GetUserIdFromClaims(User.Claims));

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // GET api/tasks/latest
        [HttpGet]
        [Route("latest")]
        public async Task<IActionResult> GetLatestTasks()
        {
            var result = await _taskSrv.GetLatestTask(_helper.GetUserIdFromClaims(User.Claims));

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // POST api/tasks
        [HttpPost]
        public async Task<IActionResult> AddTasks([FromBody]TaskToManipulateDTO item)
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user.IsChild)
            {
                return StatusCode(403, "You have no rights to do this.");
            }

            var result = _taskSrv.AddTasks(user.Id, item);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // PUT api/tasks/{taskId}
        [HttpPut]
        [Route("{taskId}")]
        public async Task<IActionResult> EditTask(string taskId, [FromBody]TaskToManipulateDTO item)
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user.IsChild)
            {
                return StatusCode(403, "You have no rights to do this.");
            }

            var result = _taskSrv.EditTask(user.Id, taskId, item);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
        
        // DELETE api/tasks
        [HttpDelete("{taskId}")]
        public async Task<IActionResult> RemoveTasks(string taskId)
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user.IsChild)
            {
                return StatusCode(403, "You have no rights to do this.");
            }

            var result = _taskSrv.RemoveTask(user.Id, taskId);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
