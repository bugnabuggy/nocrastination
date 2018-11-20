using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.Entities;

namespace Nocrastination.Interfaces
{
    public interface IUserService
    {
        Task<AppUser> FindUserById(string userId);
        Task<AppUser> FindParentByChildId(string userId);
        Task<AppUser> FindChildByParentId(string userId);
    }
}
