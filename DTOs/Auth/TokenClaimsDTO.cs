namespace devhouse.DTOs;

public class TokenClaimsDTO
{
    public string roleName { get; set; } = string.Empty;
    public int developerId { get; set; }
    public int teamId { get; set; }
    public int roleId { get; set; }
}