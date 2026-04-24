using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationsController : ControllerBase
{
    private readonly IRegistrationService _registrationService;

    public RegistrationsController(IRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Registration>>> GetAllRegistrations()
    {
        var registrations = await _registrationService.GetAllRegistrationsAsync();
        return Ok(registrations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Registration>> GetRegistrationById(int id)
    {
        var registration = await _registrationService.GetRegistrationByIdAsync(id);
        if (registration == null)
            return NotFound();

        return Ok(registration);
    }

    [HttpGet("bystudent/{studentId}")]
    public async Task<ActionResult<Registration>> GetRegistrationByStudent(int studentId)
    {
        var registration = await _registrationService.GetRegistrationByStudentIdAsync(studentId);
        if (registration == null)
            return NotFound();

        return Ok(registration);
    }

    [HttpGet("registered")]
    public async Task<ActionResult<IEnumerable<Registration>>> GetRegisteredStudents()
    {
        var registrations = await _registrationService.GetRegisteredStudentsAsync();
        return Ok(registrations);
    }

    [HttpGet("unregistered")]
    public async Task<ActionResult<IEnumerable<Registration>>> GetUnregisteredStudents()
    {
        var registrations = await _registrationService.GetUnregisteredStudentsAsync();
        return Ok(registrations);
    }

    [HttpPost]
    public async Task<ActionResult<Registration>> CreateRegistration(Registration registration)
    {
        var createdRegistration = await _registrationService.CreateRegistrationAsync(registration);
        return CreatedAtAction(nameof(GetRegistrationById), new { id = createdRegistration.Id }, createdRegistration);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRegistration(int id, Registration registration)
    {
        if (id != registration.Id)
            return BadRequest();

        var existingRegistration = await _registrationService.GetRegistrationByIdAsync(id);
        if (existingRegistration == null)
            return NotFound();

        await _registrationService.UpdateRegistrationAsync(registration);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRegistration(int id)
    {
        var registration = await _registrationService.GetRegistrationByIdAsync(id);
        if (registration == null)
            return NotFound();

        await _registrationService.DeleteRegistrationAsync(id);
        return NoContent();
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterStudent([FromQuery] int studentId, [FromQuery] string registrationLocation, [FromQuery] string registrar)
    {
        try
        {
            await _registrationService.RegisterStudentAsync(studentId, registrationLocation, registrar);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("cancel/{studentId}")]
    public async Task<IActionResult> CancelRegistration(int studentId)
    {
        try
        {
            await _registrationService.CancelRegistrationAsync(studentId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}