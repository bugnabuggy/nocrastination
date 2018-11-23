using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Nocrastination.Core.Entities;

namespace Nocrastination.Data
{
    public class ApplicationDbContext: IdentityDbContext<AppUser>
    {
        public DbSet<ChildTask> ChildTasks { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
