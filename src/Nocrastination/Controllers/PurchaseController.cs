using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Helpers;
using Nocrastination.Interfaces;

namespace Nocrastination.Controllers
{
    [Route("api/item")]
    [Authorize]
    public class PurchaseController : Controller
    {
        private IPurchaseService _purchaseSrv;
        private IClaimsHelper _helper;

        public PurchaseController
            (IPurchaseService purchaseSrv,
            IClaimsHelper helper)
        {
            _purchaseSrv = purchaseSrv;
            _helper = helper;
        }

        [HttpGet]
        [Route("status")]
        public async Task<IActionResult> GetChildStatusAsync()
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user != null )
            {
                 return Ok(_purchaseSrv.GetStatus(user));
            }

            return StatusCode(403);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersItem()
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user != null && user.IsChild)
            {
                return Ok(_purchaseSrv.GetItems(user));
            }

            return StatusCode(403);
        }

        [HttpPut]
        [Route("{itemId}")]
        public async Task<IActionResult> SelectItem(string itemId)
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user != null && user.IsChild)
            {
                var result = _purchaseSrv.SelectItem(user.Id, itemId);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return StatusCode(403);
        }
    }
}
