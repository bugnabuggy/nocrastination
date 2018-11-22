using Nocrastination.Core.Entities;
using Nocrastination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Data;

namespace Nocrastination.Services
{
    public class StoreService : IStoreService
    {
        private IRepository<StoreItem> _storeRepo;

        public StoreService(IRepository<StoreItem> storeRepo)
        {
            _storeRepo = storeRepo;
        }

        public IEnumerable<StoreItem> GetAllItemsInStore()
        {
            return _storeRepo.Data;
        }

        public bool IsStoreItemExists(string itemId, out StoreItem item)
        {
            item = null;

            if (Guid.TryParse(itemId, out Guid id))
            {
                item = _storeRepo.Data.FirstOrDefault(x => x.Id == id);

                if (item != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
