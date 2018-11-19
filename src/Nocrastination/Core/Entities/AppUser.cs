using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Core.Entities
{
    public class AppUser: IdentityUser
    {
        public string FullName { get; set; }
        public bool IsChild { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public int Points { get; set; }
        public string ParentId { get; set; }
    }
}
