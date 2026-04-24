namespace UniversityAdmissionSystem.Core.Models;

public class Dormitory
{
    public int Id { get; set; }
    public string BuildingNumber { get; set; } = string.Empty;
    public string RoomNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string RoomType { get; set; } = string.Empty;
    public int MaxCapacity { get; set; }
    public int CurrentOccupancy { get; set; }
    public string? Facilities { get; set; }
    public string? Status { get; set; }
    public ICollection<Student>? Students { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}