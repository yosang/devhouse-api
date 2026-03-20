using System.ComponentModel.DataAnnotations;

namespace devhouse.DTOs;

public class UpdateRoleDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string? Name { get; set; }
}