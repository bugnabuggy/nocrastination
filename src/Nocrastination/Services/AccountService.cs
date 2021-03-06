﻿using Nocrastination.Core.DTO;
using Nocrastination.Interfaces;
using Nocrastination.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nocrastination.Core.Entities;

namespace Nocrastination.Services
{
    public class AccountService : IAccountService
    {
        private UserManager<AppUser> _userManager;
        private IPurchaseService _purchaseService;

        public AccountService(UserManager<AppUser> userManager,
                IPurchaseService purchaseService
        )
        {
            _userManager = userManager;
            _purchaseService = purchaseService;
        }

        public async Task<OperationResult> Register(RegisterUserDTO regUser)
        {
            if (IsUserExists(regUser, out string item))
            {
                return new OperationResult
                {
                    Data = new[] { regUser },
                    Success = false,
                    Messages = new[] { $"{item} is invalid or was already taken" }
                };
            }

            var newUser = new AppUser()
            {
                Email = regUser.Email,
                UserName = regUser.UserName,
                FullName = regUser.FullName
            };

            var result = await _userManager.CreateAsync(newUser, regUser.Password);

            if (result.Succeeded)
            {
                return new OperationResult()
                {
					Data = new [] { newUser },
                    Messages = new[] { "User was successfully registered." },
                    Success = true
                };
            }

            return new OperationResult()
            {
                Messages = new[] { "Something was failed while user registration." }
            };
        }

        public async Task<OperationResult> Register(RegisterChildDTO regChild, string id)
        {
            if (IsUserExists(regChild, out string item))
            {
                return new OperationResult
                {
                    Data = new[] { regChild },
                    Success = false,
                    Messages = new[] { $"{item} is invalid or was already taken" }
                };
            }

            var newUser = new AppUser()
            {
                Email = regChild.Email,
                UserName = regChild.UserName,
                FullName = regChild.FullName,
                Age = regChild.Age,
                Sex = regChild.Sex,
                IsChild = true,
				ParentId = id
            };

            var result = await _userManager.CreateAsync(newUser, regChild.Password);

            if (result.Succeeded)
            {
                _purchaseService.SetInitialItem(newUser.Id);

                return new OperationResult()
                {
                    Messages = new[] { "User was successfully registered." },
                    Success = true
                };
            }

            return new OperationResult()
            {
                Messages = new[] { "Something was failed while user registration." }
            };
        }

	    public async Task<AppUser> GetUser(string id)
	    {
		    var user = await _userManager.FindByIdAsync(id);
		    return user;
	    }

	    private bool IsUserExists(RegisterUserDTO user, out string item)
        {
            if (_userManager.FindByNameAsync(user.UserName).Result != null)
            {
                item = "UserName";
                return true;
            }
            item = "";
            return false;
        }
    }
}
