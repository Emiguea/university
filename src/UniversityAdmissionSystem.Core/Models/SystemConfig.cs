namespace UniversityAdmissionSystem.Core.Models;

public class SystemConfig
{
    public int Id { get; set; }
    public string ConfigKey { get; set; } = string.Empty;
    public string? ConfigValue { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}