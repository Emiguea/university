using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IStatisticsService
{
    Task<int> GetTotalAdmittedStudentsAsync();
    Task<int> GetTotalRegisteredStudentsAsync();
    Task<decimal> GetRegistrationRateAsync();
    Task<int> GetTotalPaidStudentsAsync();
    Task<decimal> GetPaymentRateAsync();
    Task<decimal> GetTotalPaymentAmountAsync();
    Task<Dictionary<string, int>> GetAdmissionsByDepartmentAsync();
    Task<Dictionary<string, int>> GetAdmissionsByMajorAsync();
    Task<Dictionary<string, int>> GetRegistrationsByDepartmentAsync();
    Task<Dictionary<string, int>> GetPaymentsByDepartmentAsync();
    Task<Dictionary<string, int>> GetDormitoryOccupancyAsync();
    Task<Dictionary<string, int>> GetClassOccupancyAsync();
    Task<IEnumerable<Student>> GetStudentsWithPendingRegistrationAsync();
    Task<IEnumerable<Student>> GetStudentsWithPendingPaymentAsync();
}