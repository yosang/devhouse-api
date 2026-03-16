namespace devhouse.Models;

public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public IEnumerable<Developer>? Developers { get; set; }
}