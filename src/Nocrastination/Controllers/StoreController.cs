using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nocrastination.Core.Entities;
using Nocrastination.Interfaces;
using Nocrastination.Helpers;

namespace Nocrastination.Controllers
{
    [Route("api/store")]
    public class StoreController : Controller
    {
        private IStoreService _storeSrv;
        private IPurchaseService _purchaseSrv;
        private IClaimsHelper _helper;

        public StoreController
            (IStoreService storeSrv,
            IPurchaseService purchaseSrv,
            IClaimsHelper helper)
        {
            _storeSrv = storeSrv;
            _purchaseSrv = purchaseSrv;
            _helper = helper;
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var user = await _helper.GetUserFromClaims(User.Claims);
            var result = _storeSrv.GetAllItemsInStore().Where(x=>x.Gender.Equals(user.Sex, StringComparison.CurrentCultureIgnoreCase));

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("buy")]
        public async Task<IActionResult> BuyItemAsync([FromBody]StoreItem item)
        {
            var user = await _helper.GetUserFromClaims(User.Claims);

            if (user != null && user.IsChild)
            {
                var result = _purchaseSrv.BuyItem(user.Id, item.Id.ToString());

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
