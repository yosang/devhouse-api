namespace devhouse.DTOs;

public class ReadDeveloperDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public int TeamId { get; set; }
    public string? TeamName { get; set; }
    public string[]? Projects { get; set; }
}