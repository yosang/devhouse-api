using System.ComponentModel.DataAnnotations;

namespace devhouse.DTOs;

public class CreateProjectDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string? Name { get; set; }

    [Required]
    public int ProjectTypeId { get; set; }

    [Required]
    public int TeamId { get; set; }
}