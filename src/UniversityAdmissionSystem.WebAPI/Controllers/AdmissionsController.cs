using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdmissionsController : ControllerBase
{
    private readonly IAdmissionService _admissionService;

    public AdmissionsController(IAdmissionService admissionService)
    {
        _admissionService = admissionService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Admission>>> GetAllAdmissions()
    {
        var admissions = await _admissionService.GetAllAdmissionsAsync();
        return Ok(admissions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Admission>> GetAdmissionById(int id)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(id);
        if (admission == null)
            return NotFound();

        return Ok(admission);
    }

    [HttpGet("bystudent/{studentId}")]
    public async Task<ActionResult<Admission>> GetAdmissionByStudent(int studentId)
    {
        var admission = await _admissionService.GetAdmissionByStudentIdAsync(studentId);
        if (admission == null)
            return NotFound();

        return Ok(admission);
    }

    [HttpGet("bynumber/{admissionNumber}")]
    public async Task<ActionResult<Admission>> GetAdmissionByNumber(string admissionNumber)
    {
        var admission = await _admissionService.GetAdmissionByAdmissionNumberAsync(admissionNumber);
        if (admission == null)
            return NotFound();

        return Ok(admission);
    }

    [HttpGet("published")]
    public async Task<ActionResult<IEnumerable<Admission>>> GetPublishedAdmissions()
    {
        var admissions = await _admissionService.GetPublishedAdmissionsAsync();
        return Ok(admissions);
    }

    [HttpGet("unpublished")]
    public async Task<ActionResult<IEnumerable<Admission>>> GetUnpublishedAdmissions()
    {
        var admissions = await _admissionService.GetUnpublishedAdmissionsAsync();
        return Ok(admissions);
    }

    [HttpPost]
    public async Task<ActionResult<Admission>> CreateAdmission(Admission admission)
    {
        var createdAdmission = await _admissionService.CreateAdmissionAsync(admission);
        return CreatedAtAction(nameof(GetAdmissionById), new { id = createdAdmission.Id }, createdAdmission);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAdmission(int id, Admission admission)
    {
        if (id != admission.Id)
            return BadRequest();

        var existingAdmission = await _admissionService.GetAdmissionByIdAsync(id);
        if (existingAdmission == null)
            return NotFound();

        await _admissionService.UpdateAdmissionAsync(admission);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAdmission(int id)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(id);
        if (admission == null)
            return NotFound();

        await _admissionService.DeleteAdmissionAsync(id);
        return NoContent();
    }

    [HttpPost("{admissionId}/publish")]
    public async Task<IActionResult> PublishAdmission(int admissionId)
    {
        try
        {
            await _admissionService.PublishAdmissionAsync(admissionId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{admissionId}/printnotice")]
    public async Task<IActionResult> PrintAdmissionNotice(int admissionId)
    {
        try
        {
            await _admissionService.PrintAdmissionNoticeAsync(admissionId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{admissionId}/printmailinglabel")]
    public async Task<IActionResult> PrintMailingLabel(int admissionId)
    {
        try
        {
            await _admissionService.PrintMailingLabelAsync(admissionId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{admissionId}/generateadmissionnumber")]
    public async Task<IActionResult> GenerateAdmissionNumber(int admissionId)
    {
        try
        {
            await _admissionService.GenerateAdmissionNumberAsync(admissionId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}