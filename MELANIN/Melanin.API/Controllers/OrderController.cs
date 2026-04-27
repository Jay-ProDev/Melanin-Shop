using Melanin.API.DTO.Mappers;
using Melanin.API.DTO.Request;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Melanin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // ===== ENDPOINTS CLIENT =====

    [HttpPost]
    [EndpointSummary("Passer une commande à partir du panier")]
    public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
    {
        try
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Order order = await _orderService.CreateAsync(memberId, dto.ShippingAddressId, dto.BillingAddressId, dto.CouponCode);
            return Ok(order.ToDto());
        }
        catch (EmptyCartException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (AddressNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (ProductOutOfStockException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet("my-orders")]
    [EndpointSummary("Récupérer mes commandes")]
    public async Task<IActionResult> GetMyOrders()
    {
        int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        IEnumerable<Order> orders = await _orderService.GetByMemberIdAsync(memberId);
        return Ok(orders.Select(o => o.ToDto()));
    }

    [HttpGet("my-orders/{id}")]
    [EndpointSummary("Récupérer une de mes commandes")]
    public async Task<IActionResult> GetMyOrder(int id)
    {
        try
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Order order = await _orderService.GetByIdForMemberAsync(id, memberId);
            return Ok(order.ToDto());
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPut("{id}/cancel")]
    [EndpointSummary("Annuler ma commande")]
    public async Task<IActionResult> CancelMyOrder(int id)
    {
        try
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _orderService.CancelByClientAsync(id, memberId);
            return Ok(new { Message = "Commande annulée !" });
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (InvalidOrderStatusException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }


    // ===== ENDPOINTS ADMIN =====

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("[Admin] Récupérer toutes les commandes")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Order> orders = await _orderService.GetAllAsync();
        return Ok(orders.Select(o => o.ToDto()));
    }

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("[Admin] Récupérer les commandes par statut")]
    public async Task<IActionResult> GetByStatus(OrderStatus status)
    {
        IEnumerable<Order> orders = await _orderService.GetByStatusAsync(status);
        return Ok(orders.Select(o => o.ToDto()));
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("[Admin] Récupérer une commande par son id")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            Order order = await _orderService.GetByIdAsync(id);
            return Ok(order.ToDto());
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPut("{id}/ship")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("[Admin] Marquer une commande comme expédiée")]
    public async Task<IActionResult> Ship(int id)
    {
        try
        {
            await _orderService.ShipAsync(id);
            return Ok(new { Message = "Commande marquée comme expédiée !" });
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (InvalidOrderStatusException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPut("{id}/deliver")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("[Admin] Marquer une commande comme livrée")]
    public async Task<IActionResult> Deliver(int id)
    {
        try
        {
            await _orderService.DeliverAsync(id);
            return Ok(new { Message = "Commande marquée comme livrée !" });
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (InvalidOrderStatusException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPut("{id}/admin-cancel")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("[Admin] Annuler une commande")]
    public async Task<IActionResult> AdminCancel(int id)
    {
        try
        {
            await _orderService.CancelByAdminAsync(id);
            return Ok(new { Message = "Commande annulée !" });
        }
        catch (OrderNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (InvalidOrderStatusException ex)
        {
            return BadRequest(new { ex.Message });
        }
    } 
}