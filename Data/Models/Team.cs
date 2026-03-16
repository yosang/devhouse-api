using devhouse.Models;

public class Team
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public IEnumerable<Project>? Projects { get; set; }
    public IEnumerable<Developer>? Developers { get; set; }
}