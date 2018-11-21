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
        [Route("user")]
        public async Task<IActionResult> GetUsersItem()
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user != null && user.IsChild)
            {
                return Ok(_purchaseSrv.GetItems(user.Id));
            }

            return StatusCode(403);
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyItemAsync([FromBody]string itemId)
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user != null && user.IsChild)
            {
                var result = _purchaseSrv.BuyItem(user.Id, itemId);

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
