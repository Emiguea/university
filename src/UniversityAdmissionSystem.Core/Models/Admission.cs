namespace UniversityAdmissionSystem.Core.Models;

public class Admission
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public Student? Student { get; set; }
    public string AdmissionNumber { get; set; } = string.Empty;
    public string Major { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string AdmissionType { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishDate { get; set; }
    public bool IsNoticePrinted { get; set; }
    public DateTime? NoticePrintDate { get; set; }
    public bool IsMailingLabelPrinted { get; set; }
    public DateTime? MailingLabelPrintDate { get; set; }
    public string? MailingAddress { get; set; }
    public string? MailingContact { get; set; }
    public string? MailingPhone { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
}