﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.Entities;

namespace Nocrastination.Interfaces
{
    public interface IPurchaseService
    {
        IEnumerable<Purchase> GetItems(string id);
    }
}
