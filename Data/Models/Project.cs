namespace devhouse.Models;

public class Project
{
    public int Id { get; set; }
    public string? Name { get; set; }

    // Foreign Keys
    public int ProjectTypeId { get; set; }
    public int TeamId { get; set; }

    // Navigation properties
    public ProjectType? ProjectType { get; set; }
    public Team? Team { get; set; }
}