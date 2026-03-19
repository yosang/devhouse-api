namespace devhouse.DTOs;

public class UpdateDeveloperDTO
{
    public int Id { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int TeamId { get; set; }
    public int RoleId { get; set; }
}