using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace devhouse.Models;

public class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    [JsonIgnore]
    public IEnumerable<Developer>? Developers { get; set; }
}