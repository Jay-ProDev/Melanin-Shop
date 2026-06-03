using Melanin.Domain.Entities;

namespace Melanin.Application.Interfaces.Repositories;

public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment);
    Task<Payment?> GetByStripeSessionIdAsync(string stripeSessionId);
    Task UpdateAsync(Payment payment);
}