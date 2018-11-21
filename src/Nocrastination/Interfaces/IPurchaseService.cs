using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.Entities;
using Nocrastination.Models;

namespace Nocrastination.Interfaces
{
    public interface IPurchaseService
    {
        IEnumerable<Purchase> GetItems(string childId);
        OperationResult<Purchase> BuyItem(string childId, string itemId);
    }
}
