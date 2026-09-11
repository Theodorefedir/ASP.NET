using less1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace less1
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions options):base(options){
            
        }
        public DbSet<Product> Products { get; set; }        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);

                e.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(255);

                e.Property(p => p.Description)
                .HasColumnType("text");

                e.Property(p => p.Image)
                .HasMaxLength(255);
            });

            // Category
            modelBuilder.Entity<Category>(e =>
            {
                e.HasKey(c => c.Id);

                e.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

                e.Property(c => c.Description)
                .HasColumnType("text");

                e.Property(c => c.Image)
                .HasMaxLength(255);
            });

            // Relationships
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

        }
    }
}
