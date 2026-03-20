namespace devhouse.DTOs;

public class DeveloperDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public RoleDTO? Role { get; set; }
}

public class DeveloperDetailsDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public RoleDTO? Role { get; set; }
    public TeamDTO? Team { get; set; }
    public IEnumerable<ProjectDTO>? Projects { get; set; }
}