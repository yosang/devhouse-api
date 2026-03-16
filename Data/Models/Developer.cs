namespace devhouse.Models;

public class Developer
{
    public int Id { get; set; }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }

    // FK
    public int TeamId { get; set; }
    public int RoleId { get; set; }

    // Nav props
    public Team? Team { get; set; }
    public Role? Role { get; set; }
}