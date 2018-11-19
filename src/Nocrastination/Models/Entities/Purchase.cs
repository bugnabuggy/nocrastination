using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Core.Entities
{
    public class Purchase
    {
        public Guid Id { get; set; }
        public int Points { get; set; }
        public string ChildId { get; set; }
        public Guid ItemId { get; set; }
    }
}
