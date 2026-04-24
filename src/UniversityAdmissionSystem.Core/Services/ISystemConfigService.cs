using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface ISystemConfigService
{
    Task<SystemConfig?> GetConfigByIdAsync(int id);
    Task<SystemConfig?> GetConfigByKeyAsync(string configKey);
    Task<T?> GetConfigValueAsync<T>(string configKey, T? defaultValue = default);
    Task<IEnumerable<SystemConfig>> GetAllConfigsAsync();
    Task<IEnumerable<SystemConfig>> GetConfigsByCategoryAsync(string category);
    Task<SystemConfig> CreateConfigAsync(SystemConfig config);
    Task UpdateConfigAsync(SystemConfig config);
    Task SetConfigValueAsync(string configKey, string configValue);
    Task DeleteConfigAsync(int id);
}