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
    [Range(1, 3)]
    public int TeamId { get; set; }

    [Range(1, 3)]
    [Required]
    public int RoleId { get; set; }
}