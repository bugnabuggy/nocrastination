using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nocrastination.Core.Entities;
using Nocrastination.Settings;

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

		public static IEnumerable<Claim> GetClaims(IEnumerable<string> roles, string username, string id)
		{
			var claims = new List<Claim>();
			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}


			claims.Add(new Claim("aud", IdentityServerSettings.Api));
			claims.Add(new Claim("aud", "http://localhost:53200/resources"));
			claims.Add(new Claim("client_id", IdentityServerSettings.Client));
			claims.Add(new Claim("sub", id));
			Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			claims.Add(new Claim("auth_time", unixTimestamp.ToString()));
			claims.Add(new Claim("idp", "local"));
			claims.Add(new Claim("scope", "offline_access"));
			claims.Add(new Claim("scope", IdentityServerSettings.Api));
			claims.Add(new Claim("amr", "pwd"));
			return claims;
		}
	}
}
