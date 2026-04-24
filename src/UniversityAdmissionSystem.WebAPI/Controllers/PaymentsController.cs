using Microsoft.AspNetCore.Mvc;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments()
    {
        var payments = await _paymentService.GetAllPaymentsAsync();
        return Ok(payments);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Payment>> GetPaymentById(int id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    [HttpGet("bystudent/{studentId}")]
    public async Task<ActionResult<Payment>> GetPaymentByStudent(int studentId)
    {
        var payment = await _paymentService.GetPaymentByStudentIdAsync(studentId);
        if (payment == null)
            return NotFound();

        return Ok(payment);
    }

    [HttpGet("paid")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPaidPayments()
    {
        var payments = await _paymentService.GetPaidPaymentsAsync();
        return Ok(payments);
    }

    [HttpGet("unpaid")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetUnpaidPayments()
    {
        var payments = await _paymentService.GetUnpaidPaymentsAsync();
        return Ok(payments);
    }

    [HttpGet("partialpaid")]
    public async Task<ActionResult<IEnumerable<Payment>>> GetPartialPaidPayments()
    {
        var payments = await _paymentService.GetPartialPaidPaymentsAsync();
        return Ok(payments);
    }

    [HttpPost]
    public async Task<ActionResult<Payment>> CreatePayment(Payment payment)
    {
        var createdPayment = await _paymentService.CreatePaymentAsync(payment);
        return CreatedAtAction(nameof(GetPaymentById), new { id = createdPayment.Id }, createdPayment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayment(int id, Payment payment)
    {
        if (id != payment.Id)
            return BadRequest();

        var existingPayment = await _paymentService.GetPaymentByIdAsync(id);
        if (existingPayment == null)
            return NotFound();

        await _paymentService.UpdatePaymentAsync(payment);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id);
        if (payment == null)
            return NotFound();

        await _paymentService.DeletePaymentAsync(id);
        return NoContent();
    }

    [HttpPost("record")]
    public async Task<IActionResult> RecordPayment(
        [FromQuery] int studentId,
        [FromQuery] decimal amount,
        [FromQuery] string paymentMethod,
        [FromQuery] string? transactionId = null)
    {
        try
        {
            await _paymentService.RecordPaymentAsync(studentId, amount, paymentMethod, transactionId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("{paymentId}/updatestatus")]
    public async Task<IActionResult> UpdatePaymentStatus(int paymentId)
    {
        try
        {
            await _paymentService.UpdatePaymentStatusAsync(paymentId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}