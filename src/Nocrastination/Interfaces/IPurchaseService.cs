using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.Entities;
using Nocrastination.Models;
using Nocrastination.Models.DTO;

namespace Nocrastination.Interfaces
{
    public interface IPurchaseService
    {
        IEnumerable<OutfitDTO> GetItems(AppUser child);
        OperationResult<Purchase> BuyItem(string childId, string itemId);
        OperationResult SelectItem(string childId, string itemId);
        UserStatusDTO GetStatus(AppUser childId);
        OperationResult SetInitialItem(string childId);
    }
}
