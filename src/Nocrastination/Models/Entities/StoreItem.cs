using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Core.Entities
{
    public class StoreItem
    {
        public Guid Id { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
    }
}
