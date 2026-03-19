namespace devhouse.DTOs;

public class ReadProjectDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ProjectType { get; set; }
    public TeamDTO? Team { get; set; }
}