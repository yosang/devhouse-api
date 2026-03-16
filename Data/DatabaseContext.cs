using devhouse.Models;
using Microsoft.EntityFrameworkCore;

namespace devhouse.DatabaseContext;

public class DatabaseContext : DbContext
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectType> ProjectTypes { get; set; }

    // DI Constructor
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);

        // Implement releationships
    }
}