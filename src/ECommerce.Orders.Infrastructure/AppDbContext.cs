using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Orders.Infrastructure;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderLine> OrderLines { get; set; }
    public DbSet<OrderSummary> OrderSummaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            // Relatie configuratie met OrderLine
            entity.HasMany<OrderLine>()
                  .WithOne(ol => ol.Product)
                  .HasForeignKey(ol => ol.ProductId);

            // Property configuratie
            entity.Property(p => p.IsAvailable)
                  .HasDefaultValue(true); 
        });

        modelBuilder.Entity<Order>()
            .HasMany(o => o.OrderLines)
            .WithOne(ol => ol.Order)
            .HasForeignKey(ol => ol.OrderId);

        modelBuilder.Entity<OrderSummary>(entity =>
        {
            entity.HasKey(o => o.Id);
        });
    }
}
