namespace devhouse.DTOs;

public class TeamDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<DeveloperDTO>? Developers { get; set; }
}