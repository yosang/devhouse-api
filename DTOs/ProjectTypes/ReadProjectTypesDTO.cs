namespace devhouse.DTOs;

public class ProjectTypesDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class ProjectTypesDetailsDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<ProjectForProjectTypesDTO>? Projects { get; set; }
}