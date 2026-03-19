using devhouse.DTOs;

public class ReadTeamDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<ProjectDTO>? Projects { get; set; }
    public IEnumerable<DeveloperDTO>? Developers { get; set; }
}