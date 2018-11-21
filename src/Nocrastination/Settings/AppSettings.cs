using Nocrastination.Core.Entities;
using Nocrastination.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Settings
{
    public class AppSettings
    {
        public static StoreSettings Store { get; set; }
        public static StoreItemDTO[] StoreItems { get; set; }
    }

    public class StoreSettings
    {
        public static double PointsMultiplier { get; set; }
    }
}
