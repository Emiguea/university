using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class StudentService : IStudentService
{
    private readonly IRepository<Student> _studentRepository;
    private readonly IRepository<ClassInfo> _classInfoRepository;
    private readonly IRepository<Dormitory> _dormitoryRepository;
    private readonly ApplicationDbContext _context;

    public StudentService(
        IRepository<Student> studentRepository,
        IRepository<ClassInfo> classInfoRepository,
        IRepository<Dormitory> dormitoryRepository,
        ApplicationDbContext context)
    {
        _studentRepository = studentRepository;
        _classInfoRepository = classInfoRepository;
        _dormitoryRepository = dormitoryRepository;
        _context = context;
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.ClassInfo)
            .Include(s => s.Dormitory)
            .Include(s => s.Admission)
            .Include(s => s.Registration)
            .Include(s => s.Payment)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Student?> GetStudentByStudentNumberAsync(string studentNumber)
    {
        return await _context.Students
            .Include(s => s.ClassInfo)
            .Include(s => s.Dormitory)
            .Include(s => s.Admission)
            .Include(s => s.Registration)
            .Include(s => s.Payment)
            .FirstOrDefaultAsync(s => s.StudentNumber == studentNumber);
    }

    public async Task<IEnumerable<Student>> GetAllStudentsAsync()
    {
        return await _context.Students
            .Include(s => s.ClassInfo)
            .Include(s => s.Dormitory)
            .Include(s => s.Admission)
            .Include(s => s.Registration)
            .Include(s => s.Payment)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetStudentsByClassIdAsync(int classId)
    {
        return await _context.Students
            .Include(s => s.ClassInfo)
            .Include(s => s.Dormitory)
            .Include(s => s.Admission)
            .Include(s => s.Registration)
            .Include(s => s.Payment)
            .Where(s => s.ClassId == classId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetStudentsByDormitoryIdAsync(int dormitoryId)
    {
        return await _context.Students
            .Include(s => s.ClassInfo)
            .Include(s => s.Dormitory)
            .Include(s => s.Admission)
            .Include(s => s.Registration)
            .Include(s => s.Payment)
            .Where(s => s.DormitoryId == dormitoryId)
            .ToListAsync();
    }

    public async Task<Student> CreateStudentAsync(Student student)
    {
        student.CreatedAt = DateTime.Now;
        await _studentRepository.AddAsync(student);
        await _studentRepository.SaveChangesAsync();
        return student;
    }

    public async Task UpdateStudentAsync(Student student)
    {
        student.UpdatedAt = DateTime.Now;
        _studentRepository.Update(student);
        await _studentRepository.SaveChangesAsync();
    }

    public async Task DeleteStudentAsync(int id)
    {
        var student = await _studentRepository.GetByIdAsync(id);
        if (student != null)
        {
            _studentRepository.Delete(student);
            await _studentRepository.SaveChangesAsync();
        }
    }

    public async Task AssignClassAsync(int studentId, int classId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        var classInfo = await _classInfoRepository.GetByIdAsync(classId);

        if (student == null || classInfo == null)
            throw new KeyNotFoundException("Student or class not found");

        if (classInfo.CurrentStudents >= classInfo.MaxStudents)
            throw new InvalidOperationException("Class is full");

        if (student.ClassId.HasValue)
        {
            var oldClass = await _classInfoRepository.GetByIdAsync(student.ClassId.Value);
            if (oldClass != null)
            {
                oldClass.CurrentStudents--;
                _classInfoRepository.Update(oldClass);
            }
        }

        student.ClassId = classId;
        student.UpdatedAt = DateTime.Now;
        _studentRepository.Update(student);

        classInfo.CurrentStudents++;
        _classInfoRepository.Update(classInfo);

        await _studentRepository.SaveChangesAsync();
    }

    public async Task AssignDormitoryAsync(int studentId, int dormitoryId)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);

        if (student == null || dormitory == null)
            throw new KeyNotFoundException("Student or dormitory not found");

        if (dormitory.CurrentOccupancy >= dormitory.MaxCapacity)
            throw new InvalidOperationException("Dormitory is full");

        if (student.DormitoryId.HasValue)
        {
            var oldDormitory = await _dormitoryRepository.GetByIdAsync(student.DormitoryId.Value);
            if (oldDormitory != null)
            {
                oldDormitory.CurrentOccupancy--;
                _dormitoryRepository.Update(oldDormitory);
            }
        }

        student.DormitoryId = dormitoryId;
        student.UpdatedAt = DateTime.Now;
        _studentRepository.Update(student);

        dormitory.CurrentOccupancy++;
        _dormitoryRepository.Update(dormitory);

        await _studentRepository.SaveChangesAsync();
    }

    public async Task GenerateStudentNumberAsync(int studentId)
    {
        var student = await _context.Students
            .Include(s => s.Admission)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            throw new KeyNotFoundException("Student not found");

        if (!string.IsNullOrEmpty(student.StudentNumber))
            return;

        var year = DateTime.Now.Year.ToString();
        var maxStudentNumber = await _context.Students
            .Where(s => s.StudentNumber.StartsWith(year))
            .Select(s => s.StudentNumber)
            .OrderByDescending(s => s)
            .FirstOrDefaultAsync();

        int sequence = 1;
        if (!string.IsNullOrEmpty(maxStudentNumber) && maxStudentNumber.Length > 4)
        {
            var sequencePart = maxStudentNumber.Substring(4);
            if (int.TryParse(sequencePart, out var seq))
            {
                sequence = seq + 1;
            }
        }

        student.StudentNumber = $"{year}{sequence:D6}";
        student.UpdatedAt = DateTime.Now;
        _studentRepository.Update(student);
        await _studentRepository.SaveChangesAsync();
    }
}