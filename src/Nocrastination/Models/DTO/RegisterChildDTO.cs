using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Core.DTO
{
    public class RegisterChildDTO: RegisterUserDTO
    {
        public int Age { get; set; }
        public string Sex { get; set; }
    }
}
