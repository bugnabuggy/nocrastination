using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nocrastination.Core.Entities;

namespace Nocrastination.Helpers
{
    public class ClaimsHelper : IClaimsHelper
    {
        private UserManager<AppUser> _userManager;

        public ClaimsHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public string GetUserIdFromClaims(IEnumerable<Claim> claims)
        {
            var result = claims.FirstOrDefault(c => c.Type == "sub");

            return result.Value;
        }

        public async Task<AppUser> GetUserFromClaims(IEnumerable<Claim> claims)
        {
            var Id = GetUserIdFromClaims(claims);

            var user = await _userManager.FindByIdAsync(Id);

            return user;
        }
    }
}
