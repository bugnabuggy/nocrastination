using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Models
{
    public class OperationResult
    {
        public virtual IEnumerable<object> Data { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public bool Success { get; set; }
    }

    public class OperationResult<T>
    {
        public virtual IEnumerable<T> Data { get; set; }
        public IEnumerable<string> Messages { get; set; }
        public bool Success { get; set; }
    }
}
