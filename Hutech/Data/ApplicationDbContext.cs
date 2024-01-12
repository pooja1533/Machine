using Hutech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Hutech.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions):base(dbContextOptions) { }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        //public System.Data.Entity.DbSet<UserDetail> UserDetail { get; set; }
    }
}
