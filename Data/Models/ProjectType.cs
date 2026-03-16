using System.Text.Json.Serialization;

namespace devhouse.Models;

public class ProjectType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual IEnumerable<Project>? Projects { get; set; }
}