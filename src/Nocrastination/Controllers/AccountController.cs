using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Nocrastination.Helpers;
using Nocrastination.Interfaces;

namespace Nocrastination.Controllers
{
    [Route("api/register")]
    public class AccountController :Controller
    {
        private IAccountService _accSrv;
        private IClaimsHelper _helper;

        public AccountController
            (IAccountService accSrv, 
            IClaimsHelper helper)
        {
            _accSrv = accSrv;
            _helper = helper;
        }

        // POST api/register
        [HttpPost]
        public async Task<IActionResult> RegisterParent([FromBody] RegisterUserDTO user)
        {
            var result = await _accSrv.Register(user);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        // POST api/register/child
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RegisterChild([FromBody] RegisterChildDTO child)
        {
            var user = _helper.GetUserFromClaims(User.Claims);

            if (!user.IsChild)
            {
                return StatusCode(401, "You have no rights to do this.");
            }

            var result = await _accSrv.Register(child, user.Id);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
