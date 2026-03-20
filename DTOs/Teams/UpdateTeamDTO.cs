using System.ComponentModel.DataAnnotations;

namespace devhouse.DTOs;

public class UpdateTeamDTO
{

    [Required]
    public int Id { get; set; }

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string? Name { get; set; }
}