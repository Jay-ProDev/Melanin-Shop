using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using System.Collections.Generic;

namespace Melanin.Application.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetByMemberIdAsync(int memberId);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
    Task<Order> AddAsync(Order order);
    Task UpdateAsync(Order order);
}