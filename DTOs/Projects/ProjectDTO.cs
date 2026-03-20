namespace devhouse.DTOs;

public class ProjectDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ProjectTypesDTO? ProjectType { get; set; }
}

public class ProjectDetailsDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ProjectTypesDTO? ProjectType { get; set; }
    public TeamDTO? Team { get; set; }
}
public class ProjectForProjectTypesDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public TeamDTO? Team { get; set; }
}