using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Models.DTO
{
    public class UserStatusDTO
    {
        public int Score { get; set; }
        public string ItemName { get; set; }
        public string ItemImageUrl { get; set; }
    }
}
