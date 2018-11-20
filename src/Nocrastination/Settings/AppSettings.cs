using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Settings
{
    public class AppSettings
    {
        public static StoreSettings Store { get; set; }
    }

    public class StoreSettings
    {
        public static double PointsMultiplier { get; set; }
    }
}
