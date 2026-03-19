namespace devhouse.DTOs;

public class ReadDeveloperDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public TeamDTO? Team { get; set; }
    public IEnumerable<ProjectDTO>? Projects { get; set; }
}