using UniversityAdmissionSystem.Core.Models;

namespace UniversityAdmissionSystem.Core.Services;

public interface IPaymentService
{
    Task<Payment?> GetPaymentByIdAsync(int id);
    Task<Payment?> GetPaymentByStudentIdAsync(int studentId);
    Task<IEnumerable<Payment>> GetAllPaymentsAsync();
    Task<IEnumerable<Payment>> GetPaidPaymentsAsync();
    Task<IEnumerable<Payment>> GetUnpaidPaymentsAsync();
    Task<IEnumerable<Payment>> GetPartialPaidPaymentsAsync();
    Task<Payment> CreatePaymentAsync(Payment payment);
    Task UpdatePaymentAsync(Payment payment);
    Task DeletePaymentAsync(int id);
    Task RecordPaymentAsync(int studentId, decimal amount, string paymentMethod, string? transactionId = null);
    Task UpdatePaymentStatusAsync(int paymentId);
}