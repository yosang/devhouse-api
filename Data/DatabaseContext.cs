using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Context;

public class DatabaseContext : DbContext
{
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Developer> Developers { get; set; }

        // DI Constructor
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder mb)
        {
                base.OnModelCreating(mb);

                // Basic property requirements
                mb.Entity<Project>().Property(p => p.Name).IsRequired();
                mb.Entity<ProjectType>().Property(p => p.Name).IsRequired();
                mb.Entity<Team>().Property(p => p.Name).IsRequired();
                mb.Entity<Developer>().Property(p => p.Firstname).IsRequired();
                mb.Entity<Developer>().Property(p => p.Lastname).IsRequired();
                mb.Entity<Role>().Property(p => p.Name).IsRequired();



                // Relationships
                mb.Entity<Project>() // A Project belongs to one project type - A Project type can be set on many Projects
                        .HasOne(pt => pt.ProjectType)
                        .WithMany(p => p.Projects)
                        .HasForeignKey(fk => fk.ProjectTypeId)
                        .OnDelete(DeleteBehavior.NoAction);

                mb.Entity<Project>() // A Project belongs to one team - A Team can have many Projects
                        .HasOne(t => t.Team)
                        .WithMany(p => p.Projects)
                        .HasForeignKey(fk => fk.TeamId)
                        .OnDelete(DeleteBehavior.NoAction);

                mb.Entity<Developer>() // A developer belongs to one team - A team can have many developers
                        .HasOne(t => t.Team)
                        .WithMany(d => d.Developers)
                        .HasForeignKey(fk => fk.TeamId)
                        .OnDelete(DeleteBehavior.NoAction);

                mb.Entity<Developer>() // A developer has one role - A role can be set on many developers
                        .HasOne(t => t.Role)
                        .WithMany(d => d.Developers)
                        .HasForeignKey(fk => fk.RoleId)
                        .OnDelete(DeleteBehavior.NoAction);
        }
}