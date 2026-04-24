using Microsoft.EntityFrameworkCore;
using UniversityAdmissionSystem.Core.Models;
using UniversityAdmissionSystem.Core.Repositories;
using UniversityAdmissionSystem.Core.Services;
using UniversityAdmissionSystem.Infrastructure.Data;

namespace UniversityAdmissionSystem.Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly IRepository<Payment> _paymentRepository;
    private readonly IRepository<Student> _studentRepository;
    private readonly ApplicationDbContext _context;

    public PaymentService(
        IRepository<Payment> paymentRepository,
        IRepository<Student> studentRepository,
        ApplicationDbContext context)
    {
        _paymentRepository = paymentRepository;
        _studentRepository = studentRepository;
        _context = context;
    }

    public async Task<Payment?> GetPaymentByIdAsync(int id)
    {
        return await _context.Payments
            .Include(p => p.Student)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Payment?> GetPaymentByStudentIdAsync(int studentId)
    {
        return await _context.Payments
            .Include(p => p.Student)
            .FirstOrDefaultAsync(p => p.StudentId == studentId);
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync()
    {
        return await _context.Payments
            .Include(p => p.Student)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPaidPaymentsAsync()
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Where(p => p.PaidAmount >= p.TotalAmount && p.TotalAmount > 0)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetUnpaidPaymentsAsync()
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Where(p => p.PaidAmount == 0)
            .ToListAsync();
    }

    public async Task<IEnumerable<Payment>> GetPartialPaidPaymentsAsync()
    {
        return await _context.Payments
            .Include(p => p.Student)
            .Where(p => p.PaidAmount > 0 && p.PaidAmount < p.TotalAmount)
            .ToListAsync();
    }

    public async Task<Payment> CreatePaymentAsync(Payment payment)
    {
        payment.RemainingAmount = payment.TotalAmount - payment.PaidAmount;
        payment.PaymentStatus = DeterminePaymentStatus(payment.PaidAmount, payment.TotalAmount);
        payment.CreatedAt = DateTime.Now;
        await _paymentRepository.AddAsync(payment);
        await _paymentRepository.SaveChangesAsync();
        return payment;
    }

    public async Task UpdatePaymentAsync(Payment payment)
    {
        payment.RemainingAmount = payment.TotalAmount - payment.PaidAmount;
        payment.PaymentStatus = DeterminePaymentStatus(payment.PaidAmount, payment.TotalAmount);
        payment.UpdatedAt = DateTime.Now;
        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync();
    }

    public async Task DeletePaymentAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        if (payment != null)
        {
            _paymentRepository.Delete(payment);
            await _paymentRepository.SaveChangesAsync();
        }
    }

    public async Task RecordPaymentAsync(int studentId, decimal amount, string paymentMethod, string? transactionId = null)
    {
        var student = await _studentRepository.GetByIdAsync(studentId);
        if (student == null)
            throw new KeyNotFoundException("Student not found");

        var existingPayment = await _context.Payments
            .FirstOrDefaultAsync(p => p.StudentId == studentId);

        if (existingPayment == null)
            throw new KeyNotFoundException("Payment record not found for student");

        existingPayment.PaidAmount += amount;
        existingPayment.RemainingAmount = existingPayment.TotalAmount - existingPayment.PaidAmount;
        existingPayment.PaymentDate = DateTime.Now;
        existingPayment.PaymentMethod = paymentMethod;
        existingPayment.TransactionId = transactionId;
        existingPayment.PaymentStatus = DeterminePaymentStatus(existingPayment.PaidAmount, existingPayment.TotalAmount);
        existingPayment.UpdatedAt = DateTime.Now;

        _paymentRepository.Update(existingPayment);
        await _paymentRepository.SaveChangesAsync();
    }

    public async Task UpdatePaymentStatusAsync(int paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
            throw new KeyNotFoundException("Payment not found");

        payment.PaymentStatus = DeterminePaymentStatus(payment.PaidAmount, payment.TotalAmount);
        payment.UpdatedAt = DateTime.Now;
        _paymentRepository.Update(payment);
        await _paymentRepository.SaveChangesAsync();
    }

    private string DeterminePaymentStatus(decimal paidAmount, decimal totalAmount)
    {
        if (paidAmount >= totalAmount && totalAmount > 0)
            return "已付清";
        else if (paidAmount > 0)
            return "部分付款";
        else
            return "未付款";
    }
}