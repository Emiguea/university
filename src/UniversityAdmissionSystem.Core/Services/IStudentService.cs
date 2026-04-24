using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IStudentService
{
    Task<Student?> GetStudentByIdAsync(int id);
    Task<Student?> GetStudentByStudentNumberAsync(string studentNumber);
    Task<IEnumerable<Student>> GetAllStudentsAsync();
    Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId);
    Task<IEnumerable<Student>> GetStudentsByDormitoryIdAsync(int dormitoryId);
    Task<Student> CreateStudentAsync(Student student);
    Task UpdateStudentAsync(Student student);
    Task DeleteStudentAsync(int id);
    Task AssignClassAsync(int studentId, int classId);
    Task AssignDormitoryAsync(int studentId, int dormitoryId);
    Task GenerateStudentNumberAsync(int studentId);
}