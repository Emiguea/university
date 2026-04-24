using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly IClassInfoService _classInfoService;

    public ClassesController(IClassInfoService classInfoService)
    {
        _classInfoService = classInfoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassInfo>>> GetAllClasses()
    {
        var classes = await _classInfoService.GetAllClassInfosAsync();
        return Ok(classes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClassInfo>> GetClassById(int id)
    {
        var classInfo = await _classInfoService.GetClassInfoByIdAsync(id);
        if (classInfo == null)
            return NotFound();

        return Ok(classInfo);
    }

    [HttpGet("bycode/{classCode}")]
    public async Task<ActionResult<ClassInfo>> GetClassByCode(string classCode)
    {
        var classInfo = await _classInfoService.GetClassInfoByClassCodeAsync(classCode);
        if (classInfo == null)
            return NotFound();

        return Ok(classInfo);
    }

    [HttpGet("bydepartment/{department}")]
    public async Task<ActionResult<IEnumerable<ClassInfo>>> GetClassesByDepartment(string department)
    {
        var classes = await _classInfoService.GetClassInfosByDepartmentAsync(department);
        return Ok(classes);
    }

    [HttpGet("bymajor/{major}")]
    public async Task<ActionResult<IEnumerable<ClassInfo>>> GetClassesByMajor(string major)
    {
        var classes = await _classInfoService.GetClassInfosByMajorAsync(major);
        return Ok(classes);
    }

    [HttpGet("bygrade/{grade}")]
    public async Task<ActionResult<IEnumerable<ClassInfo>>> GetClassesByGrade(int grade)
    {
        var classes = await _classInfoService.GetClassInfosByGradeAsync(grade);
        return Ok(classes);
    }

    [HttpPost]
    public async Task<ActionResult<ClassInfo>> CreateClass(ClassInfo classInfo)
    {
        var createdClass = await _classInfoService.CreateClassInfoAsync(classInfo);
        return CreatedAtAction(nameof(GetClassById), new { id = createdClass.Id }, createdClass);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClass(int id, ClassInfo classInfo)
    {
        if (id != classInfo.Id)
            return BadRequest();

        var existingClass = await _classInfoService.GetClassInfoByIdAsync(id);
        if (existingClass == null)
            return NotFound();

        await _classInfoService.UpdateClassInfoAsync(classInfo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        try
        {
            var classInfo = await _classInfoService.GetClassInfoByIdAsync(id);
            if (classInfo == null)
                return NotFound();

            await _classInfoService.DeleteClassInfoAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{classId}/isfull")]
    public async Task<ActionResult<bool>> IsClassFull(int classId)
    {
        try
        {
            var isFull = await _classInfoService.IsClassFullAsync(classId);
            return Ok(isFull);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("{classId}/availableslots")]
    public async Task<ActionResult<int>> GetAvailableSlots(int classId)
    {
        try
        {
            var slots = await _classInfoService.GetAvailableSlotsAsync(classId);
            return Ok(slots);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}