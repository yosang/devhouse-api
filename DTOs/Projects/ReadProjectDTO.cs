namespace devhouse.DTOs;

public class ReadProjectDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int ProjectTypeId { get; set; }
    public int TeamId { get; set; }
}