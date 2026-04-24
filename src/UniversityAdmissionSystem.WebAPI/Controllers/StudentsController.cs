using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();
        return Ok(students);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();

        return Ok(student);
    }

    [HttpGet("bynumber/{studentNumber}")]
    public async Task<ActionResult<Student>> GetStudentByNumber(string studentNumber)
    {
        var student = await _studentService.GetStudentByStudentNumberAsync(studentNumber);
        if (student == null)
            return NotFound();

        return Ok(student);
    }

    [HttpGet("byclass/{classId}")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByClass(int classId)
    {
        var students = await _studentService.GetStudentsByClassIdAsync(classId);
        return Ok(students);
    }

    [HttpGet("bydormitory/{dormitoryId}")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudentsByDormitory(int dormitoryId)
    {
        var students = await _studentService.GetStudentsByDormitoryIdAsync(dormitoryId);
        return Ok(students);
    }

    [HttpPost]
    public async Task<ActionResult<Student>> CreateStudent(Student student)
    {
        var createdStudent = await _studentService.CreateStudentAsync(student);
        return CreatedAtAction(nameof(GetStudentById), new { id = createdStudent.Id }, createdStudent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, Student student)
    {
        if (id != student.Id)
            return BadRequest();

        var existingStudent = await _studentService.GetStudentByIdAsync(id);
        if (existingStudent == null)
            return NotFound();

        await _studentService.UpdateStudentAsync(student);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return NotFound();

        await _studentService.DeleteStudentAsync(id);
        return NoContent();
    }

    [HttpPost("{studentId}/assignclass/{classId}")]
    public async Task<IActionResult> AssignClass(int studentId, int classId)
    {
        try
        {
            await _studentService.AssignClassAsync(studentId, classId);
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

    [HttpPost("{studentId}/assigndormitory/{dormitoryId}")]
    public async Task<IActionResult> AssignDormitory(int studentId, int dormitoryId)
    {
        try
        {
            await _studentService.AssignDormitoryAsync(studentId, dormitoryId);
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

    [HttpPost("{studentId}/generatestudentnumber")]
    public async Task<IActionResult> GenerateStudentNumber(int studentId)
    {
        try
        {
            await _studentService.GenerateStudentNumberAsync(studentId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}