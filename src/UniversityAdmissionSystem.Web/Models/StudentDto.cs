namespace UniversityAdmissionSystem.Web.Models;

public class StudentDto
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
    public int? DormitoryId { get; set; }
    public AdmissionDto? Admission { get; set; }
    public RegistrationDto? Registration { get; set; }
    public PaymentDto? Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class AdmissionDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public StudentDto? Student { get; set; }
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
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class RegistrationDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? RegistrationLocation { get; set; }
    public string? Registrar { get; set; }
    public bool IsRegistered { get; set; }
    public string? Notes { get; set; }
    public string? DocumentsReceived { get; set; }
    public string? SpecialRequirements { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PaymentDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string PaymentType { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? TransactionId { get; set; }
    public string? InvoiceNumber { get; set; }
    public string? PaymentStatus { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}