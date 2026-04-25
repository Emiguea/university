using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IClassInfoService
{
    Task<ClassInfo?> GetClassInfoByIdAsync(int id);
    Task<ClassInfo?> GetClassInfoByClassCodeAsync(string classCode);
    Task<IEnumerable<ClassInfo>> GetAllClassInfosAsync();
    Task<IEnumerable<ClassInfo>> GetClassInfosByDepartmentAsync(string department);
    Task<IEnumerable<ClassInfo>> GetClassInfosByMajorAsync(string major);
    Task<IEnumerable<ClassInfo>> GetClassInfosByGradeAsync(int grade);
    Task<ClassInfo> CreateClassInfoAsync(ClassInfo classInfo);
    Task UpdateClassInfoAsync(ClassInfo classInfo);
    Task DeleteClassInfoAsync(int id);
    Task<bool> IsClassFullAsync(int classId);
    Task<int> GetAvailableSlotsAsync(int classId);
}