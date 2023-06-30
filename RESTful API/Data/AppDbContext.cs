using RESTful_API.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace RESTful_API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Supply> Supplies { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<OrderSupply> OrderSupply { get; set; }
    public DbSet<OrderSubscribe> OrderSubscribes { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderSupply>()
            .HasKey(os => new { os.Id });

        modelBuilder.Entity<OrderSupply>()
            .HasOne(os => os.Order)
            .WithMany(o => o.OrderSupply)
            .HasForeignKey(os => os.OrderId);

        modelBuilder.Entity<OrderSupply>()
            .HasOne(os => os.Supply)
            .WithMany(s => s.OrderSupply)
            .HasForeignKey(os => os.SupplyId);
    }
}
