using MarketPlaceCore.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketPlaceCore.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasConversion<double>(); // SQLite doesn't support decimal natively well for all operations, but EF handles it.
            // Actually for SQLite decimal is fine usually but double is safer for some functions.
            // Let's stick to default and see.
    }
}
