using ECartApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ECartApp.DAL
{
    public class ECartContext : IdentityDbContext<ApplicationUser>
    {
        public ECartContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Items> Items { get; set; }
    }
}
