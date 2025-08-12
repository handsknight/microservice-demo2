using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data;
public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> opts) : base(opts) { }
    public DbSet<Order> Orders => Set<Order>();
}
