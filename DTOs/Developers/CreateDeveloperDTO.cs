using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace devhouse.DTOs;

public class CreateDeveloperDTO
{
    [Required]
    public string? Firstname { get; set; }

    [Required]
    public string? Lastname { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [MinLength(8)]
    public string? Password { get; set; }

    [Required]
    public int TeamId { get; set; }

    [Required]
    public int RoleId { get; set; }
}