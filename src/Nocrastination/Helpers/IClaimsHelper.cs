using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Nocrastination.Core.Entities;

namespace Nocrastination.Helpers
{
    public interface IClaimsHelper
    {
        string GetUserIdFromClaims(IEnumerable<Claim> claims);
        AppUser GetUserFromClaims(IEnumerable<Claim> claims);
    }
}
