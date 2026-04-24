using UniversityAdmissionSystem.Infrastructure.Security;

namespace UniversityAdmissionSystem.WebAPI.Dtos;

public class StudentResponseDto
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
    public string? ClassName { get; set; }
    public int? DormitoryId { get; set; }
    public string? DormitoryNumber { get; set; }
    public AdmissionResponseDto? Admission { get; set; }
    public RegistrationResponseDto? Registration { get; set; }
    public PaymentResponseDto? Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public static StudentResponseDto FromEntity(Core.Models.Student student, bool maskSensitiveData = true)
    {
        var dto = new StudentResponseDto
        {
            Id = student.Id,
            Name = student.Name,
            Gender = student.Gender,
            BirthDate = student.BirthDate,
            IdCardNumber = maskSensitiveData ? DataMaskingHelper.MaskIdCard(student.IdCardNumber) : student.IdCardNumber,
            PhoneNumber = maskSensitiveData ? DataMaskingHelper.MaskPhoneNumber(student.PhoneNumber) : student.PhoneNumber,
            Email = maskSensitiveData ? DataMaskingHelper.MaskEmail(student.Email) : student.Email,
            Address = maskSensitiveData ? DataMaskingHelper.MaskAddress(student.Address) : student.Address,
            StudentNumber = student.StudentNumber,
            ClassId = student.ClassId,
            ClassName = student.ClassInfo?.ClassName,
            DormitoryId = student.DormitoryId,
            DormitoryNumber = student.Dormitory != null ? $"{student.Dormitory.BuildingNumber}-{student.Dormitory.RoomNumber}" : null,
            CreatedAt = student.CreatedAt,
            UpdatedAt = student.UpdatedAt
        };

        if (student.Admission != null)
        {
            dto.Admission = AdmissionResponseDto.FromEntity(student.Admission);
        }

        if (student.Registration != null)
        {
            dto.Registration = RegistrationResponseDto.FromEntity(student.Registration);
        }

        if (student.Payment != null)
        {
            dto.Payment = PaymentResponseDto.FromEntity(student.Payment);
        }

        return dto;
    }
}

public class AdmissionResponseDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
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

    public static AdmissionResponseDto FromEntity(Core.Models.Admission admission, bool maskSensitiveData = true)
    {
        return new AdmissionResponseDto
        {
            Id = admission.Id,
            StudentId = admission.StudentId,
            AdmissionNumber = admission.AdmissionNumber,
            Major = admission.Major,
            Department = admission.Department,
            AdmissionType = admission.AdmissionType,
            AdmissionDate = admission.AdmissionDate,
            IsPublished = admission.IsPublished,
            PublishDate = admission.PublishDate,
            IsNoticePrinted = admission.IsNoticePrinted,
            NoticePrintDate = admission.NoticePrintDate,
            IsMailingLabelPrinted = admission.IsMailingLabelPrinted,
            MailingLabelPrintDate = admission.MailingLabelPrintDate,
            MailingAddress = maskSensitiveData ? DataMaskingHelper.MaskAddress(admission.MailingAddress ?? string.Empty) : admission.MailingAddress,
            MailingContact = admission.MailingContact,
            MailingPhone = maskSensitiveData ? DataMaskingHelper.MaskPhoneNumber(admission.MailingPhone ?? string.Empty) : admission.MailingPhone
        };
    }
}

public class RegistrationResponseDto
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

    public static RegistrationResponseDto FromEntity(Core.Models.Registration registration)
    {
        return new RegistrationResponseDto
        {
            Id = registration.Id,
            StudentId = registration.StudentId,
            RegistrationDate = registration.RegistrationDate,
            RegistrationLocation = registration.RegistrationLocation,
            Registrar = registration.Registrar,
            IsRegistered = registration.IsRegistered,
            Notes = registration.Notes,
            DocumentsReceived = registration.DocumentsReceived,
            SpecialRequirements = registration.SpecialRequirements
        };
    }
}

public class PaymentResponseDto
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

    public static PaymentResponseDto FromEntity(Core.Models.Payment payment)
    {
        return new PaymentResponseDto
        {
            Id = payment.Id,
            StudentId = payment.StudentId,
            PaymentType = payment.PaymentType,
            TotalAmount = payment.TotalAmount,
            PaidAmount = payment.PaidAmount,
            RemainingAmount = payment.RemainingAmount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod,
            TransactionId = payment.TransactionId,
            InvoiceNumber = payment.InvoiceNumber,
            PaymentStatus = payment.PaymentStatus,
            Notes = payment.Notes
        };
    }
}
