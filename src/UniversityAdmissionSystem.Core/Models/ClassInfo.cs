namespace UniversityAdmissionSystem.Core.Models;

public class ClassInfo
{
    public int Id { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public string ClassCode { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public int Grade { get; set; }
    public int MaxStudents { get; set; }
    public int CurrentStudents { get; set; }
    public string? HeadTeacher { get; set; }
    public string? HeadTeacherPhone { get; set; }
    public ICollection<Student>? Students { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}