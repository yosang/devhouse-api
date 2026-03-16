namespace devhouse.Models;

public class ProjectType
{
    public int Id { get; set; }
    public string? Name { get; set; }

    // Navigation properties
    public IEnumerable<Project>? Projects { get; set; }
}