using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using Nocrastination.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Nocrastination.Core.Entities;
using Nocrastination.Helpers;
using Nocrastination.Interfaces;
using Nocrastination.Settings;

namespace Nocrastination.Controllers
{
	[Route("api/register")]
	public class AccountController : Controller
	{
		private IAccountService _accSrv;
		private IClaimsHelper _helper;
		private IdentityServerTools _identityServer;

		public AccountController
				(IAccountService accSrv,
				IClaimsHelper helper,
				IdentityServerTools identityServer)
		{
			_accSrv = accSrv;
			_helper = helper;
			_identityServer = identityServer;
		}

		// POST api/register
		[HttpPost]
		public async Task<IActionResult> RegisterParent([FromBody] RegisterUserDTO user)
		{
			var result = await _accSrv.Register(user);

			if (result.Success)
			{
				var createdUser = result.Data.FirstOrDefault();
				var id = (createdUser as AppUser)?.Id;
				var claims = ClaimsHelper.GetClaims(new List<string>(), user.UserName, id);
				var jwt = await this._identityServer.IssueJwtAsync(IdentityServerSettings.AccessTokenLifetime, claims);

				result.Data = new [] { new { access_token = jwt} } ;
				return Ok(result);
			}

			return BadRequest(result);
		}

		[HttpGet("test")]
		[Authorize]
		public IActionResult Test()
		{
			return Ok("yes");
		}

		// POST api/register/child
		[Authorize]
		[HttpPost("child")]
		public async Task<IActionResult> RegisterChild([FromBody] RegisterChildDTO child)
		{
			var user = await _helper.GetUserFromClaims(User.Claims);

			if (user.IsChild)
			{
				return StatusCode(403, "You have no rights to do this.");
			}

			var result = await _accSrv.Register(child, user.Id);

			if (result.Success)
			{
				return Ok(result);
			}

			return BadRequest(result);
		}
	}
}
