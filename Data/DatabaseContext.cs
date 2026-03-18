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
                mb.Entity<Developer>().HasIndex(p => p.Email).IsUnique();
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

                // Seeds

                // Project Types
                mb.Entity<ProjectType>().HasData(
                    new ProjectType { Id = 1, Name = "Web Application" },
                    new ProjectType { Id = 2, Name = "Mobile App" },
                    new ProjectType { Id = 3, Name = "API Service" }
                );

                // Roles
                mb.Entity<Role>().HasData(
                    new Role { Id = 1, Name = "Developer" },
                    new Role { Id = 2, Name = "TeamLead" },
                    new Role { Id = 3, Name = "Admin" }
                );

                // Teams
                mb.Entity<Team>().HasData(
                    new Team { Id = 1, Name = "Platform" },
                    new Team { Id = 2, Name = "Mobile" },
                    new Team { Id = 3, Name = "API" }
                );

                // Developers
                mb.Entity<Developer>().HasData(
                    new Developer { Id = 1, Firstname = "Alice", Lastname = "Johnson", Email = "alice@dev.com", Password = "developer1234", TeamId = 1, RoleId = 1 },
                    new Developer { Id = 2, Firstname = "Michael", Lastname = "Cross", Email = "michael@dev.com", Password = "teamlead1234", TeamId = 1, RoleId = 2 },
                    new Developer { Id = 3, Firstname = "Elise", Lastname = "Bergum", Email = "elise@dev.com", Password = "admin1234", TeamId = 1, RoleId = 3 },
                    new Developer { Id = 4, Firstname = "Bob", Lastname = "Smith", TeamId = 2, RoleId = 1 },
                    new Developer { Id = 5, Firstname = "Marta", Lastname = "Parks", TeamId = 2, RoleId = 2 },
                    new Developer { Id = 6, Firstname = "Diana", Lastname = "Clark", TeamId = 3, RoleId = 1 }
                );

                // Projects
                mb.Entity<Project>().HasData(
                    new Project { Id = 1, Name = "Website", ProjectTypeId = 1, TeamId = 1 },
                    new Project { Id = 2, Name = "Mobile", ProjectTypeId = 2, TeamId = 2 },
                    new Project { Id = 3, Name = "API", ProjectTypeId = 3, TeamId = 3 }
                );
        }
}