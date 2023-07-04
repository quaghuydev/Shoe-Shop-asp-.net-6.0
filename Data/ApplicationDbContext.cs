using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShoeShop.Models;

namespace ShoeShop.Dao
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}
