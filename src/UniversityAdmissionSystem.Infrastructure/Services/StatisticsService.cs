using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class StatisticsService : IStatisticsService
{
    private readonly ApplicationDbContext _context;

    public StatisticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalAdmittedStudentsAsync()
    {
        return await _context.Admissions.CountAsync();
    }

    public async Task<int> GetTotalRegisteredStudentsAsync()
    {
        return await _context.Registrations
            .Where(r => r.IsRegistered)
            .CountAsync();
    }

    public async Task<decimal> GetRegistrationRateAsync()
    {
        var totalAdmitted = await GetTotalAdmittedStudentsAsync();
        if (totalAdmitted == 0)
            return 0;

        var totalRegistered = await GetTotalRegisteredStudentsAsync();
        return Math.Round((decimal)totalRegistered / totalAdmitted * 100, 2);
    }

    public async Task<int> GetTotalPaidStudentsAsync()
    {
        return await _context.Payments
            .Where(p => p.PaidAmount >= p.TotalAmount && p.TotalAmount > 0)
            .CountAsync();
    }

    public async Task<decimal> GetPaymentRateAsync()
    {
        var totalStudents = await _context.Students.CountAsync();
        if (totalStudents == 0)
            return 0;

        var totalPaid = await GetTotalPaidStudentsAsync();
        return Math.Round((decimal)totalPaid / totalStudents * 100, 2);
    }

    public async Task<decimal> GetTotalPaymentAmountAsync()
    {
        return await _context.Payments.SumAsync(p => p.PaidAmount);
    }

    public async Task<Dictionary<string, int>> GetAdmissionsByDepartmentAsync()
    {
        return await _context.Admissions
            .GroupBy(a => a.Department)
            .Select(g => new { Department = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Department ?? "未知", x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetAdmissionsByMajorAsync()
    {
        return await _context.Admissions
            .GroupBy(a => a.Major)
            .Select(g => new { Major = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Major ?? "未知", x => x.Count);
    }

    public async Task<Dictionary<string, int>> GetRegistrationsByDepartmentAsync()
    {
        var registeredStudents = await _context.Registrations
            .Include(r => r.Student)
            .ThenInclude(s => s.Admission)
            .Where(r => r.IsRegistered)
            .ToListAsync();

        return registeredStudents
            .GroupBy(r => r.Student?.Admission?.Department ?? "未知")
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetPaymentsByDepartmentAsync()
    {
        var paidStudents = await _context.Payments
            .Include(p => p.Student)
            .ThenInclude(s => s.Admission)
            .Where(p => p.PaidAmount >= p.TotalAmount && p.TotalAmount > 0)
            .ToListAsync();

        return paidStudents
            .GroupBy(p => p.Student?.Admission?.Department ?? "未知")
            .ToDictionary(g => g.Key, g => g.Count());
    }

    public async Task<Dictionary<string, int>> GetDormitoryOccupancyAsync()
    {
        var dormitories = await _context.Dormitories
            .Select(d => new { d.BuildingNumber, d.RoomNumber, d.CurrentOccupancy, d.MaxCapacity })
            .ToListAsync();

        return dormitories
            .GroupBy(d => d.BuildingNumber)
            .ToDictionary(
                g => g.Key,
                g => g.Sum(d => d.CurrentOccupancy)
            );
    }

    public async Task<Dictionary<string, int>> GetClassOccupancyAsync()
    {
        var classes = await _context.ClassInfos
            .Select(c => new { c.ClassName, c.CurrentStudents, c.MaxStudents })
            .ToListAsync();

        return classes
            .ToDictionary(
                c => c.ClassName,
                c => c.CurrentStudents
            );
    }

    public async Task<IEnumerable<Student>> GetStudentsWithPendingRegistrationAsync()
    {
        var registeredStudentIds = await _context.Registrations
            .Where(r => r.IsRegistered)
            .Select(r => r.StudentId)
            .ToListAsync();

        return await _context.Students
            .Include(s => s.Admission)
            .Where(s => !registeredStudentIds.Contains(s.Id))
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetStudentsWithPendingPaymentAsync()
    {
        return await _context.Students
            .Include(s => s.Payment)
            .Where(s => s.Payment == null || s.Payment.PaidAmount < s.Payment.TotalAmount)
            .ToListAsync();
    }
}