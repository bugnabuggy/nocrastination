using Nocrastination.Core.Entities;
using Nocrastination.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nocrastination.Data;

namespace Nocrastination.Services
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> _userManager;
        private IRepository<AppUser> _userRepo;

        public UserService
            (UserManager<AppUser> userManager,
            IRepository<AppUser> userRepo)
        {
            _userManager = userManager;
            _userRepo = userRepo;
        }

        public AppUser FindChildByParentId(string userId)
        {
            return _userRepo.Get(x => x.ParentId == userId).FirstOrDefault();
        }

        public AppUser FindParentByChildId(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> FindUserById(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }
    }
}
