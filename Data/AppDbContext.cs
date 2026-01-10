using Microsoft.EntityFrameworkCore;
using Net9RestApi.Entities;

namespace Net9RestApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AIProject> AIProjects { get; set; }
        public DbSet<Experiment> Experiments { get; set; }
        public DbSet<Metric> Metrics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Global Soft Delete Filters
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<AIProject>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Experiment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Metric>().HasQueryFilter(m => !m.IsDeleted);

            // -------- SEED DATA --------

            var seedDate = new DateTime(2026, 1, 10);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@test.com",
                    PasswordHash = "admin123",
                    Role = "Admin",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<AIProject>().HasData(
                new AIProject
                {
                    Id = 1,
                    Name = "Seed Project",
                    Description = "Initial seeded AI project",
                    UserId = 1,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Experiment>().HasData(
                new Experiment
                {
                    Id = 1,
                    Name = "Baseline Experiment",
                    AIProjectId = 1,
                    ModelName = "ResNet50",
                    Status = "Completed",
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate,
                    IsDeleted = false
                }
            );

            modelBuilder.Entity<Metric>().HasData(
                new Metric
                {
                    Id = 1,
                    Name = "Accuracy",
                    Value = 0.92,
                    ExperimentId = 1,
                    CreatedAt = seedDate,
                    UpdatedAt = seedDate,
                    IsDeleted = false
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
