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

            base.OnModelCreating(modelBuilder);
        }
    }
}
