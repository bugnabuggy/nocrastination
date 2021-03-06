﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.Entities;

namespace Nocrastination.Interfaces
{
    public interface IStoreService
    {
        IEnumerable<StoreItem> GetAllItemsInStore();
        bool IsStoreItemExists(string itemId, out StoreItem item);
    }
}
