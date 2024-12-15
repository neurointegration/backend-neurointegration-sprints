using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser, Role, Guid>(options)
    {
        public DbSet<Sprint> Sprints { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<NeuroTask> NeuroTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .Property(p => p.SectionName)
                .HasConversion<string>();

            modelBuilder.Entity<NeuroTask>()
                .Property(nt => nt.SectionName)
                .HasConversion<string>();

        }
    }
}
