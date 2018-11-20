using Nocrastination.Core.Entities;
using Nocrastination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Nocrastination.Services
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> FindChildByParentId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> FindParentByChildId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> FindUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
