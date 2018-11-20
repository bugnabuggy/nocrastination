using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Nocrastination.Interfaces;

namespace Nocrastination.Controllers
{
    [Route("api/item")]
    public class StoreController : Controller
    {
        private IStoreService _storeSrv;

        public StoreController(IStoreService storeSrv)
        {
            _storeSrv = storeSrv;
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
    }
}
