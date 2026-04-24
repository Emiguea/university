using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IDormitoryService
{
    Task<Dormitory?> GetDormitoryByIdAsync(int id);
    Task<Dormitory?> GetDormitoryByRoomNumberAsync(string buildingNumber, string roomNumber);
    Task<IEnumerable<Dormitory>> GetAllDormitoriesAsync();
    Task<IEnumerable<Dormitory>> GetDormitoriesByBuildingAsync(string buildingNumber);
    Task<IEnumerable<Dormitory>> GetAvailableDormitoriesAsync();
    Task<Dormitory> CreateDormitoryAsync(Dormitory dormitory);
    Task UpdateDormitoryAsync(Dormitory dormitory);
    Task DeleteDormitoryAsync(int id);
    Task<bool> IsDormitoryFullAsync(int dormitoryId);
    Task<int> GetAvailableBedsAsync(int dormitoryId);
}