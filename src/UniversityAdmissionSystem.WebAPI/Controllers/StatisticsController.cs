using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    [HttpGet("totaladmitted")]
    public async Task<ActionResult<int>> GetTotalAdmittedStudents()
    {
        var count = await _statisticsService.GetTotalAdmittedStudentsAsync();
        return Ok(count);
    }

    [HttpGet("totalregistered")]
    public async Task<ActionResult<int>> GetTotalRegisteredStudents()
    {
        var count = await _statisticsService.GetTotalRegisteredStudentsAsync();
        return Ok(count);
    }

    [HttpGet("registrationrate")]
    public async Task<ActionResult<decimal>> GetRegistrationRate()
    {
        var rate = await _statisticsService.GetRegistrationRateAsync();
        return Ok(rate);
    }

    [HttpGet("totalpaid")]
    public async Task<ActionResult<int>> GetTotalPaidStudents()
    {
        var count = await _statisticsService.GetTotalPaidStudentsAsync();
        return Ok(count);
    }

    [HttpGet("paymentrate")]
    public async Task<ActionResult<decimal>> GetPaymentRate()
    {
        var rate = await _statisticsService.GetPaymentRateAsync();
        return Ok(rate);
    }

    [HttpGet("totalpaymentamount")]
    public async Task<ActionResult<decimal>> GetTotalPaymentAmount()
    {
        var amount = await _statisticsService.GetTotalPaymentAmountAsync();
        return Ok(amount);
    }

    [HttpGet("admissionsbydepartment")]
    public async Task<ActionResult<Dictionary<string, int>>> GetAdmissionsByDepartment()
    {
        var stats = await _statisticsService.GetAdmissionsByDepartmentAsync();
        return Ok(stats);
    }

    [HttpGet("admissionsbymajor")]
    public async Task<ActionResult<Dictionary<string, int>>> GetAdmissionsByMajor()
    {
        var stats = await _statisticsService.GetAdmissionsByMajorAsync();
        return Ok(stats);
    }

    [HttpGet("registrationsbydepartment")]
    public async Task<ActionResult<Dictionary<string, int>>> GetRegistrationsByDepartment()
    {
        var stats = await _statisticsService.GetRegistrationsByDepartmentAsync();
        return Ok(stats);
    }

    [HttpGet("paymentsbydepartment")]
    public async Task<ActionResult<Dictionary<string, int>>> GetPaymentsByDepartment()
    {
        var stats = await _statisticsService.GetPaymentsByDepartmentAsync();
        return Ok(stats);
    }

    [HttpGet("dormitoryoccupancy")]
    public async Task<ActionResult<Dictionary<string, int>>> GetDormitoryOccupancy()
    {
        var stats = await _statisticsService.GetDormitoryOccupancyAsync();
        return Ok(stats);
    }

    [HttpGet("classoccupancy")]
    public async Task<ActionResult<Dictionary<string, int>>> GetClassOccupancy()
    {
        var stats = await _statisticsService.GetClassOccupancyAsync();
        return Ok(stats);
    }

    [HttpGet("pendingregistration")]
    public async Task<ActionResult<IEnumerable<object>>> GetStudentsWithPendingRegistration()
    {
        var students = await _statisticsService.GetStudentsWithPendingRegistrationAsync();
        var result = students.Select(s => new
        {
            s.Id,
            s.Name,
            s.StudentNumber,
            s.Gender,
            s.PhoneNumber,
            s.Admission?.Major,
            s.Admission?.Department
        });
        return Ok(result);
    }

    [HttpGet("pendingpayment")]
    public async Task<ActionResult<IEnumerable<object>>> GetStudentsWithPendingPayment()
    {
        var students = await _statisticsService.GetStudentsWithPendingPaymentAsync();
        var result = students.Select(s => new
        {
            s.Id,
            s.Name,
            s.StudentNumber,
            s.Gender,
            s.PhoneNumber,
            s.Payment?.TotalAmount,
            s.Payment?.PaidAmount,
            s.Payment?.RemainingAmount,
            s.Payment?.PaymentStatus
        });
        return Ok(result);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<object>> GetSummary()
    {
        var totalAdmitted = await _statisticsService.GetTotalAdmittedStudentsAsync();
        var totalRegistered = await _statisticsService.GetTotalRegisteredStudentsAsync();
        var registrationRate = await _statisticsService.GetRegistrationRateAsync();
        var totalPaid = await _statisticsService.GetTotalPaidStudentsAsync();
        var paymentRate = await _statisticsService.GetPaymentRateAsync();
        var totalPaymentAmount = await _statisticsService.GetTotalPaymentAmountAsync();

        return Ok(new
        {
            TotalAdmittedStudents = totalAdmitted,
            TotalRegisteredStudents = totalRegistered,
            RegistrationRate = registrationRate,
            TotalPaidStudents = totalPaid,
            PaymentRate = paymentRate,
            TotalPaymentAmount = totalPaymentAmount
        });
    }
}