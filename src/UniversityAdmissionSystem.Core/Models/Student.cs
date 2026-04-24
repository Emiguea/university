namespace UniversityAdmissionSystem.Core.Models;

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string IdCardNumber { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string StudentNumber { get; set; } = string.Empty;
    public int? ClassId { get; set; }
    public ClassInfo? ClassInfo { get; set; }
    public int? DormitoryId { get; set; }
    public Dormitory? Dormitory { get; set; }
    public Admission? Admission { get; set; }
    public Registration? Registration { get; set; }
    public Payment? Payment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}