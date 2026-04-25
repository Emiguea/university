using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IRegistrationService
{
    Task<Registration?> GetRegistrationByIdAsync(int id);
    Task<Registration?> GetRegistrationByStudentIdAsync(int studentId);
    Task<IEnumerable<Registration>> GetAllRegistrationsAsync();
    Task<IEnumerable<Registration>> GetRegisteredStudentsAsync();
    Task<IEnumerable<Registration>> GetUnregisteredStudentsAsync();
    Task<Registration> CreateRegistrationAsync(Registration registration);
    Task UpdateRegistrationAsync(Registration registration);
    Task DeleteRegistrationAsync(int id);
    Task RegisterStudentAsync(int studentId, string registrationLocation, string registrar);
    Task CancelRegistrationAsync(int studentId);
}