﻿using System;
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
        IEnumerable<Purchase> GetItems(string childId);
        OperationResult<Purchase> BuyItem(string childId, string itemId);
        OperationResult SelectItem(string childId, string itemId);
        UserStatusDTO GetStatus(string childId);
        OperationResult SetInitialItem(string childId);
    }
}
