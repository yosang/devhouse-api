using devhouse.Interfaces;

namespace devhouse.Models;

public class Developer : ITeamedUp, IRoledUp, ISelfModify
{
    public int Id { get; set; }

    public string? Firstname { get; set; }
    public string? Lastname { get; set; }

    public string? Email { get; set; }
    public string? Password { get; set; }

    // FK
    public int TeamId { get; set; }
    public int RoleId { get; set; }

    // Nav props
    public Team? Team { get; set; }
    public Role? Role { get; set; }
}