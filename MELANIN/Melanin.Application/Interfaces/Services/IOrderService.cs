using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using System.Collections.Generic;

namespace Melanin.Application.Interfaces.Services;

public interface IOrderService
{
    // Création
    Task<Order> CreateAsync(int memberId, int shippingAddressId, int? billingAddressId = null, string? couponCode = null);

    // Lecture - Admin
    Task<Order> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);

    // Lecture - Client
    Task<Order> GetByIdForMemberAsync(int id, int memberId);
    Task<IEnumerable<Order>> GetByMemberIdAsync(int memberId);

    // Transitions de statut
    Task ConfirmAsync(int orderId);
    Task ShipAsync(int orderId);
    Task DeliverAsync(int orderId);
    Task CancelByClientAsync(int orderId, int memberId);
    Task CancelByAdminAsync(int orderId);
}