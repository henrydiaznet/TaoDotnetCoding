using Microsoft.EntityFrameworkCore;
using DotnetCoding.Core.Models;

namespace DotnetCoding.Infrastructure
{
    public class EshopContext : DbContext
    {
        public EshopContext(DbContextOptions<EshopContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<ProductDetails> Products { get; set; }
        public DbSet<ProductRequest> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDetails>().ToTable("products");
            modelBuilder.Entity<ProductRequest>().ToTable("requests");
            
            modelBuilder.Entity<ProductDetails>()
                .Property(e => e.Status)
                .HasConversion(
                    v => v.ToString(),
                    v => (ProductStatus)Enum.Parse(typeof(ProductStatus), v));
            
            modelBuilder.Entity<ProductRequest>()
                .Property(e => e.RequestType)
                .HasConversion(
                    v => v.ToString(),
                    v => (ProductRequestType)Enum.Parse(typeof(ProductRequestType), v));
        }
    }
}
