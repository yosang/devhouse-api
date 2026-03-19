namespace devhouse.DTOs;

public class ReadProjectTypesDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public IEnumerable<ReadProjectDTO>? Projects { get; set; }
}