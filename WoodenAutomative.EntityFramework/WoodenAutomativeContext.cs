using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoodenAutomative.Domain.Dtos.Request.Login;
using WoodenAutomative.Domain.Models;

namespace WoodenAutomative.EntityFramework
{
    public class WoodenAutomativeContext : DbContext
    {
        public WoodenAutomativeContext(DbContextOptions<WoodenAutomativeContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
