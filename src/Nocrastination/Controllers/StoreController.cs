using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        public IActionResult GetItems()
        {
            var result = _storeSrv.GetAllItemsInStore();

            if (result != null)
            {
                return Ok(result);
            }

            return NotFound();
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
