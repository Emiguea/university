namespace UniversityAdmissionSystem.Core.Models;

public class Registration
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? RegistrationLocation { get; set; }
    public string? Registrar { get; set; }
    public bool IsRegistered { get; set; }
    public string? Notes { get; set; }
    public string? DocumentsReceived { get; set; }
    public string? SpecialRequirements { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}