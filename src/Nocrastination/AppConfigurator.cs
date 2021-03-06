﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Nocrastination.Core.Entities;
using Nocrastination.Data;
using Nocrastination.Helpers;
using Nocrastination.Interfaces;
using Nocrastination.Services;
using Nocrastination.Settings;

namespace Nocrastination
{
    public class AppConfigurator
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IRepository<AppUser>, DbRepository<AppUser>>();
            services.AddScoped<IRepository<ChildTask>, DbRepository<ChildTask>>();
            services.AddScoped<IRepository<StoreItem>, DbRepository<StoreItem>>();
            services.AddScoped<IRepository<Purchase>, DbRepository<Purchase>>();

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IClaimsHelper, ClaimsHelper>();
        }

        public void Initialize(IServiceProvider services)
        {
            var ctx = services.GetRequiredService<ApplicationDbContext>();
            var storeItems = AppSettings.StoreItems;

            foreach (var item in storeItems)
            {
                var storedItem = ctx.StoreItems.FirstOrDefault(x => x.Name == item.Name);

                if (storedItem == null)
                {
                    ctx.StoreItems.Add(new StoreItem()
                    {
                        Name = item.Name,
                        Picture = item.Picture,
                        Points = item.Points,
                        Gender = item.Gender

                    });
                    ctx.SaveChanges();
                }
            }
        }

        private bool IsItemExistByName(ApplicationDbContext context)
        {
            return false;
        }
    }
}
