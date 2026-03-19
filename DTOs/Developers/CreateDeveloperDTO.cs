namespace devhouse.DTOs;

public class CreateDeveloperDTO
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int TeamId { get; set; }
    public int RoleId { get; set; }
}