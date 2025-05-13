// filepath: /home/sprudel/goblin-market/goblin-api/Data/ProductDbContext.cs
using Microsoft.EntityFrameworkCore;
using goblin_api.Models;

namespace goblin_api.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
        }
    }
}
