using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Widget", Price = 9.99m },
            new Product { Id = 2, Name = "Gadget", Price = 19.99m }
        );
    }
}
