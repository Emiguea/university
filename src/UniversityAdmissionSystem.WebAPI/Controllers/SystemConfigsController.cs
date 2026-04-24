using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SystemConfigsController : ControllerBase
{
    private readonly ISystemConfigService _systemConfigService;

    public SystemConfigsController(ISystemConfigService systemConfigService)
    {
        _systemConfigService = systemConfigService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemConfig>>> GetAllConfigs()
    {
        var configs = await _systemConfigService.GetAllConfigsAsync();
        return Ok(configs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SystemConfig>> GetConfigById(int id)
    {
        var config = await _systemConfigService.GetConfigByIdAsync(id);
        if (config == null)
            return NotFound();

        return Ok(config);
    }

    [HttpGet("bykey/{configKey}")]
    public async Task<ActionResult<SystemConfig>> GetConfigByKey(string configKey)
    {
        var config = await _systemConfigService.GetConfigByKeyAsync(configKey);
        if (config == null)
            return NotFound();

        return Ok(config);
    }

    [HttpGet("value/{configKey}")]
    public async Task<ActionResult<string>> GetConfigValue(string configKey)
    {
        var value = await _systemConfigService.GetConfigValueAsync<string>(configKey);
        if (value == null)
            return NotFound();

        return Ok(value);
    }

    [HttpGet("bycategory/{category}")]
    public async Task<ActionResult<IEnumerable<SystemConfig>>> GetConfigsByCategory(string category)
    {
        var configs = await _systemConfigService.GetConfigsByCategoryAsync(category);
        return Ok(configs);
    }

    [HttpPost]
    public async Task<ActionResult<SystemConfig>> CreateConfig(SystemConfig config)
    {
        try
        {
            var createdConfig = await _systemConfigService.CreateConfigAsync(config);
            return CreatedAtAction(nameof(GetConfigById), new { id = createdConfig.Id }, createdConfig);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateConfig(int id, SystemConfig config)
    {
        if (id != config.Id)
            return BadRequest();

        var existingConfig = await _systemConfigService.GetConfigByIdAsync(id);
        if (existingConfig == null)
            return NotFound();

        await _systemConfigService.UpdateConfigAsync(config);
        return NoContent();
    }

    [HttpPut("setvalue/{configKey}")]
    public async Task<IActionResult> SetConfigValue(string configKey, [FromBody] string configValue)
    {
        await _systemConfigService.SetConfigValueAsync(configKey, configValue);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConfig(int id)
    {
        var config = await _systemConfigService.GetConfigByIdAsync(id);
        if (config == null)
            return NotFound();

        await _systemConfigService.DeleteConfigAsync(id);
        return NoContent();
    }
}