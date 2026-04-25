using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DormitoriesController : ControllerBase
{
    private readonly IDormitoryService _dormitoryService;

    public DormitoriesController(IDormitoryService dormitoryService)
    {
        _dormitoryService = dormitoryService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dormitory>>> GetAllDormitories()
    {
        var dormitories = await _dormitoryService.GetAllDormitoriesAsync();
        return Ok(dormitories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Dormitory>> GetDormitoryById(int id)
    {
        var dormitory = await _dormitoryService.GetDormitoryByIdAsync(id);
        if (dormitory == null)
            return NotFound();

        return Ok(dormitory);
    }

    [HttpGet("byroom/{buildingNumber}/{roomNumber}")]
    public async Task<ActionResult<Dormitory>> GetDormitoryByRoom(string buildingNumber, string roomNumber)
    {
        var dormitory = await _dormitoryService.GetDormitoryByRoomNumberAsync(buildingNumber, roomNumber);
        if (dormitory == null)
            return NotFound();

        return Ok(dormitory);
    }

    [HttpGet("bybuilding/{buildingNumber}")]
    public async Task<ActionResult<IEnumerable<Dormitory>>> GetDormitoriesByBuilding(string buildingNumber)
    {
        var dormitories = await _dormitoryService.GetDormitoriesByBuildingAsync(buildingNumber);
        return Ok(dormitories);
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Dormitory>>> GetAvailableDormitories()
    {
        var dormitories = await _dormitoryService.GetAvailableDormitoriesAsync();
        return Ok(dormitories);
    }

    [HttpPost]
    public async Task<ActionResult<Dormitory>> CreateDormitory(Dormitory dormitory)
    {
        var createdDormitory = await _dormitoryService.CreateDormitoryAsync(dormitory);
        return CreatedAtAction(nameof(GetDormitoryById), new { id = createdDormitory.Id }, createdDormitory);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDormitory(int id, Dormitory dormitory)
    {
        if (id != dormitory.Id)
            return BadRequest();

        var existingDormitory = await _dormitoryService.GetDormitoryByIdAsync(id);
        if (existingDormitory == null)
            return NotFound();

        await _dormitoryService.UpdateDormitoryAsync(dormitory);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDormitory(int id)
    {
        try
        {
            var dormitory = await _dormitoryService.GetDormitoryByIdAsync(id);
            if (dormitory == null)
                return NotFound();

            await _dormitoryService.DeleteDormitoryAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{dormitoryId}/isfull")]
    public async Task<ActionResult<bool>> IsDormitoryFull(int dormitoryId)
    {
        try
        {
            var isFull = await _dormitoryService.IsDormitoryFullAsync(dormitoryId);
            return Ok(isFull);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{dormitoryId}/availablebeds")]
    public async Task<ActionResult<int>> GetAvailableBeds(int dormitoryId)
    {
        try
        {
            var beds = await _dormitoryService.GetAvailableBedsAsync(dormitoryId);
            return Ok(beds);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}