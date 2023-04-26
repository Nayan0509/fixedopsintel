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

        public DbSet<otp> OTP { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<DistributorAdmin> DistributorAdmins { get; set; }
        public DbSet<Territory> Territories { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
            {
            base.OnModelCreating(builder);
            //.OnDelete(DeleteBehavior.Cascade)
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
                                //builder.Entity<ApplicationUser>()
                                //                                .Ignore(u => u.SecurityStamp);     
                                builder.Entity<ApplicationUser>()
                                                                .Ignore(u => u.TwoFactorEnabled);    
                                builder.Entity<ApplicationUser>()
                                                                .Ignore(u => u.UserName);

        }
        }
}
