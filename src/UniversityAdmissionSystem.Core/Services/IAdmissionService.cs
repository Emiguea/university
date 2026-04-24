using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IAdmissionService
{
    Task<Admission?> GetAdmissionByIdAsync(int id);
    Task<Admission?> GetAdmissionByStudentIdAsync(int studentId);
    Task<Admission?> GetAdmissionByAdmissionNumberAsync(string admissionNumber);
    Task<IEnumerable<Admission>> GetAllAdmissionsAsync();
    Task<IEnumerable<Admission>> GetPublishedAdmissionsAsync();
    Task<IEnumerable<Admission>> GetUnpublishedAdmissionsAsync();
    Task<Admission> CreateAdmissionAsync(Admission admission);
    Task UpdateAdmissionAsync(Admission admission);
    Task DeleteAdmissionAsync(int id);
    Task PublishAdmissionAsync(int admissionId);
    Task PrintAdmissionNoticeAsync(int admissionId);
    Task PrintMailingLabelAsync(int admissionId);
    Task GenerateAdmissionNumberAsync(int admissionId);
}