using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WoodenAutomative.Domain.Models;

namespace WoodenAutomative.EntityFramework
{

        public partial class WoodenAutomativeContext : IdentityDbContext<ApplicationUser>
        {
            public WoodenAutomativeContext(DbContextOptions<WoodenAutomativeContext> options)
                : base(options)
            { }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.AccessFailedCount);      
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.ConcurrencyStamp);   
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.LockoutEnabled);   
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.LockoutEnd);     
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.NormalizedEmail);    
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.NormalizedUserName);
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.SecurityStamp);     
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.TwoFactorEnabled);    
            builder.Entity<ApplicationUser>()
    .Ignore(u => u.UserName);
        }
        }

}
