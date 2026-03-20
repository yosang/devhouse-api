using System.ComponentModel.DataAnnotations;

namespace devhouse.DTOs;

public class CreateRoleDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(20)]
    public string? Name { get; set; }
}