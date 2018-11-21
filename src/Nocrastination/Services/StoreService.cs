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
        private IRepository<Store> _storeRepo;

        private StoreService(IRepository<Store> storeRepo)
        {
            _storeRepo = storeRepo;
        }

        public IEnumerable<Store> GetAllItemsInStore()
        {
            return _storeRepo.Data;
        }

        public bool IsStoreItemExists(string itemId, out Store item)
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
