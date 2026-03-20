using System.ComponentModel.DataAnnotations;

namespace devhouse.DTOs;

public class CreateTeamDTO
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string? Name { get; set; }
}