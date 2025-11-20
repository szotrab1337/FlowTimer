using FlowTimer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowTimer.Infrastructure.Persistence
{
    public class FlowTimerDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<WorkItem> WorkItems { get; set; }
        public DbSet<Session> Sessions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Project>(project =>
            {
                project.HasKey(x => x.Id);

                project.Property(x => x.Name)
                    .HasMaxLength(128)
                    .IsRequired();

                project.Property(x => x.Description)
                    .HasMaxLength(256);

                project.HasMany(x => x.WorkItems)
                    .WithOne(z => z.Project)
                    .HasForeignKey(x => x.ProjectId);
            });

            builder.Entity<WorkItem>(workItem =>
            {
                workItem.HasKey(x => x.Id);

                workItem.Property(x => x.Name)
                    .HasMaxLength(128)
                    .IsRequired();

                workItem.Property(x => x.Description)
                    .HasMaxLength(256);

                workItem.HasMany(x => x.Sessions)
                    .WithOne(z => z.WorkItem)
                    .HasForeignKey(x => x.WorkItemId);
            });

            builder.Entity<Session>(session =>
            {
                session.HasKey(x => x.Id);

                session.Property(x => x.StartTime)
                    .IsRequired();

                session.Ignore(x => x.Duration);
            });
        }
    }
}