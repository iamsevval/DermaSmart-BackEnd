using DermaSmart.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DermaSmart.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<SkinProfile> SkinProfiles { get; set; }
        public DbSet<RoutineStep> RoutineSteps { get; set; }
        public DbSet<TrackingLog> TrackingLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Product>().ToTable("products");
            modelBuilder.Entity<Ingredient>().ToTable("ingredients");
            modelBuilder.Entity<SkinProfile>().ToTable("SkinProfiles");
            modelBuilder.Entity<RoutineStep>().ToTable("RoutineSteps");
            modelBuilder.Entity<TrackingLog>().ToTable("TrackingLogs");
        }
    }
}