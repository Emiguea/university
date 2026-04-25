using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.WebAPI.Dtos;

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
    public async Task<ActionResult<IEnumerable<AdmissionResponseDto>>> GetAllAdmissions()
    {
        var admissions = await _admissionService.GetAllAdmissionsAsync();
        var responseDtos = admissions.Select(a => AdmissionResponseDto.FromEntity(a, maskSensitiveData: true));
        return Ok(responseDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdmissionResponseDto>> GetAdmissionById(int id)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(id);
        if (admission == null)
            return NotFound();

        var responseDto = AdmissionResponseDto.FromEntity(admission, maskSensitiveData: true);
        return Ok(responseDto);
    }

    [HttpGet("bystudent/{studentId}")]
    public async Task<ActionResult<AdmissionResponseDto>> GetAdmissionByStudent(int studentId)
    {
        var admission = await _admissionService.GetAdmissionByStudentIdAsync(studentId);
        if (admission == null)
            return NotFound();

        var responseDto = AdmissionResponseDto.FromEntity(admission, maskSensitiveData: true);
        return Ok(responseDto);
    }

    [HttpGet("bynumber/{admissionNumber}")]
    public async Task<ActionResult<AdmissionResponseDto>> GetAdmissionByNumber(string admissionNumber)
    {
        var admission = await _admissionService.GetAdmissionByAdmissionNumberAsync(admissionNumber);
        if (admission == null)
            return NotFound();

        var responseDto = AdmissionResponseDto.FromEntity(admission, maskSensitiveData: true);
        return Ok(responseDto);
    }

    [HttpGet("published")]
    public async Task<ActionResult<IEnumerable<AdmissionResponseDto>>> GetPublishedAdmissions()
    {
        var admissions = await _admissionService.GetPublishedAdmissionsAsync();
        var responseDtos = admissions.Select(a => AdmissionResponseDto.FromEntity(a, maskSensitiveData: true));
        return Ok(responseDtos);
    }

    [HttpGet("unpublished")]
    public async Task<ActionResult<IEnumerable<AdmissionResponseDto>>> GetUnpublishedAdmissions()
    {
        var admissions = await _admissionService.GetUnpublishedAdmissionsAsync();
        var responseDtos = admissions.Select(a => AdmissionResponseDto.FromEntity(a, maskSensitiveData: true));
        return Ok(responseDtos);
    }

    [HttpPost]
    public async Task<ActionResult<AdmissionResponseDto>> CreateAdmission(Admission admission)
    {
        var createdAdmission = await _admissionService.CreateAdmissionAsync(admission);
        var responseDto = AdmissionResponseDto.FromEntity(createdAdmission, maskSensitiveData: true);
        return CreatedAtAction(nameof(GetAdmissionById), new { id = createdAdmission.Id }, responseDto);
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
