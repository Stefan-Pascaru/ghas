using Microsoft.EntityFrameworkCore;
using ProductsApi.Models;

namespace ProductsApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerProductPrice> CustomerProductPrices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerProductPrice>()
            .HasKey(cpp => new { cpp.CustomerId, cpp.ProductId });

        modelBuilder.Entity<CustomerProductPrice>()
            .HasOne(cpp => cpp.Customer)
            .WithMany(c => c.CustomerProductPrices)
            .HasForeignKey(cpp => cpp.CustomerId);

        modelBuilder.Entity<CustomerProductPrice>()
            .HasOne(cpp => cpp.Product)
            .WithMany()
            .HasForeignKey(cpp => cpp.ProductId);

        modelBuilder.Entity<CustomerProductPrice>()
            .Property(cpp => cpp.AgreedPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);
    }
}
