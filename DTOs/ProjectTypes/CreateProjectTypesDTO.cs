using System.ComponentModel.DataAnnotations;

namespace devhouse.DTOs;

public class CreateProjectTypeDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string? Name { get; set; }
}