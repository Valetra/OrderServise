using apiForRadBot.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data;

public class AppDbContext : DbContext
{
    public DbSet<Supply> Supplies { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
