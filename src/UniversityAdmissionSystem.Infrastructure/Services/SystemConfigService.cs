using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class SystemConfigService : ISystemConfigService
{
    private readonly IRepository<SystemConfig> _configRepository;
    private readonly ApplicationDbContext _context;

    public SystemConfigService(
        IRepository<SystemConfig> configRepository,
        ApplicationDbContext context)
    {
        _configRepository = configRepository;
        _context = context;
    }

    public async Task<SystemConfig?> GetConfigByIdAsync(int id)
    {
        return await _configRepository.GetByIdAsync(id);
    }

    public async Task<SystemConfig?> GetConfigByKeyAsync(string configKey)
    {
        return await _context.SystemConfigs
            .FirstOrDefaultAsync(c => c.ConfigKey == configKey);
    }

    public async Task<T?> GetConfigValueAsync<T>(string configKey, T? defaultValue = default)
    {
        var config = await GetConfigByKeyAsync(configKey);
        if (config == null || string.IsNullOrEmpty(config.ConfigValue))
            return defaultValue;

        try
        {
            return (T)Convert.ChangeType(config.ConfigValue, typeof(T));
        }
        catch
        {
            return defaultValue;
        }
    }

    public async Task<IEnumerable<SystemConfig>> GetAllConfigsAsync()
    {
        return await _configRepository.GetAllAsync();
    }

    public async Task<IEnumerable<SystemConfig>> GetConfigsByCategoryAsync(string category)
    {
        return await _context.SystemConfigs
            .Where(c => c.Category == category)
            .ToListAsync();
    }

    public async Task<SystemConfig> CreateConfigAsync(SystemConfig config)
    {
        var existingConfig = await _context.SystemConfigs
            .FirstOrDefaultAsync(c => c.ConfigKey == config.ConfigKey);

        if (existingConfig != null)
            throw new InvalidOperationException("Config key already exists");

        config.CreatedAt = DateTime.Now;
        await _configRepository.AddAsync(config);
        await _configRepository.SaveChangesAsync();
        return config;
    }

    public async Task UpdateConfigAsync(SystemConfig config)
    {
        var existingConfig = await _configRepository.GetByIdAsync(config.Id);
        if (existingConfig == null)
            throw new KeyNotFoundException("Config not found");

        existingConfig.ConfigValue = config.ConfigValue;
        existingConfig.Description = config.Description;
        existingConfig.Category = config.Category;
        existingConfig.UpdatedAt = DateTime.Now;

        _configRepository.Update(existingConfig);
        await _configRepository.SaveChangesAsync();
    }

    public async Task SetConfigValueAsync(string configKey, string configValue)
    {
        var config = await GetConfigByKeyAsync(configKey);
        if (config == null)
        {
            config = new SystemConfig
            {
                ConfigKey = configKey,
                ConfigValue = configValue,
                CreatedAt = DateTime.Now
            };
            await _configRepository.AddAsync(config);
        }
        else
        {
            config.ConfigValue = configValue;
            config.UpdatedAt = DateTime.Now;
            _configRepository.Update(config);
        }
        await _configRepository.SaveChangesAsync();
    }

    public async Task DeleteConfigAsync(int id)
    {
        var config = await _configRepository.GetByIdAsync(id);
        if (config != null)
        {
            _configRepository.Delete(config);
            await _configRepository.SaveChangesAsync();
        }
    }
}