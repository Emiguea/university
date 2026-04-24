using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class ClassInfoService : IClassInfoService
{
    private readonly IRepository<ClassInfo> _classInfoRepository;
    private readonly ApplicationDbContext _context;

    public ClassInfoService(
        IRepository<ClassInfo> classInfoRepository,
        ApplicationDbContext context)
    {
        _classInfoRepository = classInfoRepository;
        _context = context;
    }

    public async Task<ClassInfo?> GetClassInfoByIdAsync(int id)
    {
        return await _context.ClassInfos
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<ClassInfo?> GetClassInfoByClassCodeAsync(string classCode)
    {
        return await _context.ClassInfos
            .Include(c => c.Students)
            .FirstOrDefaultAsync(c => c.ClassCode == classCode);
    }

    public async Task<IEnumerable<ClassInfo>> GetAllClassInfosAsync()
    {
        return await _context.ClassInfos
            .Include(c => c.Students)
            .ToListAsync();
    }

    public async Task<IEnumerable<ClassInfo>> GetClassInfosByDepartmentAsync(string department)
    {
        return await _context.ClassInfos
            .Include(c => c.Students)
            .Where(c => c.Department == department)
            .ToListAsync();
    }

    public async Task<IEnumerable<ClassInfo>> GetClassInfosByMajorAsync(string major)
    {
        return await _context.ClassInfos
            .Include(c => c.Students)
            .Where(c => c.Major == major)
            .ToListAsync();
    }

    public async Task<IEnumerable<ClassInfo>> GetClassInfosByGradeAsync(int grade)
    {
        return await _context.ClassInfos
            .Include(c => c.Students)
            .Where(c => c.Grade == grade)
            .ToListAsync();
    }

    public async Task<ClassInfo> CreateClassInfoAsync(ClassInfo classInfo)
    {
        classInfo.CreatedAt = DateTime.Now;
        classInfo.CurrentStudents = 0;
        await _classInfoRepository.AddAsync(classInfo);
        await _classInfoRepository.SaveChangesAsync();
        return classInfo;
    }

    public async Task UpdateClassInfoAsync(ClassInfo classInfo)
    {
        classInfo.UpdatedAt = DateTime.Now;
        _classInfoRepository.Update(classInfo);
        await _classInfoRepository.SaveChangesAsync();
    }

    public async Task DeleteClassInfoAsync(int id)
    {
        var classInfo = await _classInfoRepository.GetByIdAsync(id);
        if (classInfo != null)
        {
            if (classInfo.CurrentStudents > 0)
                throw new InvalidOperationException("Cannot delete class with students");

            _classInfoRepository.Delete(classInfo);
            await _classInfoRepository.SaveChangesAsync();
        }
    }

    public async Task<bool> IsClassFullAsync(int classId)
    {
        var classInfo = await _classInfoRepository.GetByIdAsync(classId);
        if (classInfo == null)
            throw new KeyNotFoundException("Class not found");

        return classInfo.CurrentStudents >= classInfo.MaxStudents;
    }

    public async Task<int> GetAvailableSlotsAsync(int classId)
    {
        var classInfo = await _classInfoRepository.GetByIdAsync(classId);
        if (classInfo == null)
            throw new KeyNotFoundException("Class not found");

        return classInfo.MaxStudents - classInfo.CurrentStudents;
    }
}