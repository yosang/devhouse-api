using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Context;

public class DatabaseContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectType> ProjectTypes { get; set; }

    // DI Constructor
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // Property/Key requirements
        mb.Entity<Project>().Property(p => p.Name).IsRequired();
        mb.Entity<ProjectType>().Property(p => p.Name).IsRequired();


        // Relationships
        mb.Entity<Project>()
                .HasOne(pt => pt.ProjectType)
                .WithMany(p => p.Projects)
                .HasForeignKey(fk => fk.ProjectTypeId)
                .OnDelete(DeleteBehavior.NoAction);
    }
}