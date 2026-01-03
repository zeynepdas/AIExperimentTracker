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
    }
}