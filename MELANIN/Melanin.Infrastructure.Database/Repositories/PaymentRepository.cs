using Melanin.Application.Interfaces.Repositories;
using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Melanin.Infrastructure.Database.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly MelaninDbContext _dbContext;

    public PaymentRepository(MelaninDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Payment> AddAsync(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
        await _dbContext.SaveChangesAsync();

        return payment;
    }

    public async Task<Payment?> GetByStripeSessionIdAsync(string stripeSessionId)
    {
        return await _dbContext.Payments
            .Include(p => p.Order)
            .FirstOrDefaultAsync(p => p.StripeSessionId == stripeSessionId);
    }

    public async Task UpdateAsync(Payment payment)
    {
        _dbContext.Payments.Update(payment);
        await _dbContext.SaveChangesAsync();
    }
}