namespace devhouse.DTOs;

public class CreateProjectDTO
{
    public string? Name { get; set; }
    public int ProjectTypeId { get; set; }
    public int TeamId { get; set; }
}