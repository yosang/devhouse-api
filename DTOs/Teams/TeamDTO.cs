namespace devhouse.DTOs;

public class TeamDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
}
public class TeamWithDevelopersDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<DeveloperDTO>? Developers { get; set; }
}

public class TeamDetailsDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<ProjectDTO>? Projects { get; set; }
    public IEnumerable<DeveloperDTO>? Developers { get; set; }
}