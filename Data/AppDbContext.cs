using DermaSmart.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DermaSmart.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Arkadaşın mevcut tabloları
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<SkinProfile> SkinProfiles { get; set; }
        public DbSet<RoutineStep> RoutineSteps { get; set; }

        // Senin auth tabloların (ayrı)
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppSkinProfile> AppSkinProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().ToTable("AppUsers");
            modelBuilder.Entity<AppSkinProfile>().ToTable("AppSkinProfiles");

            // Mevcut tabloları olduğu gibi eşle
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Ingredient>().ToTable("ingredients");
            modelBuilder.Entity<SkinProfile>().ToTable("SkinProfiles");
            modelBuilder.Entity<RoutineStep>().ToTable("RoutineSteps");
        }
    }
}