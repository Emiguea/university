using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class DormitoryService : IDormitoryService
{
    private readonly IRepository<Dormitory> _dormitoryRepository;
    private readonly ApplicationDbContext _context;

    public DormitoryService(
        IRepository<Dormitory> dormitoryRepository,
        ApplicationDbContext context)
    {
        _dormitoryRepository = dormitoryRepository;
        _context = context;
    }

    public async Task<Dormitory?> GetDormitoryByIdAsync(int id)
    {
        return await _context.Dormitories
            .Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Dormitory?> GetDormitoryByRoomNumberAsync(string buildingNumber, string roomNumber)
    {
        return await _context.Dormitories
            .Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.BuildingNumber == buildingNumber && d.RoomNumber == roomNumber);
    }

    public async Task<IEnumerable<Dormitory>> GetAllDormitoriesAsync()
    {
        return await _context.Dormitories
            .Include(d => d.Students)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dormitory>> GetDormitoriesByBuildingAsync(string buildingNumber)
    {
        return await _context.Dormitories
            .Include(d => d.Students)
            .Where(d => d.BuildingNumber == buildingNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<Dormitory>> GetAvailableDormitoriesAsync()
    {
        return await _context.Dormitories
            .Include(d => d.Students)
            .Where(d => d.CurrentOccupancy < d.MaxCapacity)
            .ToListAsync();
    }

    public async Task<Dormitory> CreateDormitoryAsync(Dormitory dormitory)
    {
        dormitory.CreatedAt = DateTime.Now;
        dormitory.CurrentOccupancy = 0;
        await _dormitoryRepository.AddAsync(dormitory);
        await _dormitoryRepository.SaveChangesAsync();
        return dormitory;
    }

    public async Task UpdateDormitoryAsync(Dormitory dormitory)
    {
        dormitory.UpdatedAt = DateTime.Now;
        _dormitoryRepository.Update(dormitory);
        await _dormitoryRepository.SaveChangesAsync();
    }

    public async Task DeleteDormitoryAsync(int id)
    {
        var dormitory = await _dormitoryRepository.GetByIdAsync(id);
        if (dormitory != null)
        {
            if (dormitory.CurrentOccupancy > 0)
                throw new InvalidOperationException("Cannot delete dormitory with students");

            _dormitoryRepository.Delete(dormitory);
            await _dormitoryRepository.SaveChangesAsync();
        }
    }

    public async Task<bool> IsDormitoryFullAsync(int dormitoryId)
    {
        var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);
        if (dormitory == null)
            throw new KeyNotFoundException("Dormitory not found");

        return dormitory.CurrentOccupancy >= dormitory.MaxCapacity;
    }

    public async Task<int> GetAvailableBedsAsync(int dormitoryId)
    {
        var dormitory = await _dormitoryRepository.GetByIdAsync(dormitoryId);
        if (dormitory == null)
            throw new KeyNotFoundException("Dormitory not found");

        return dormitory.MaxCapacity - dormitory.CurrentOccupancy;
    }
}