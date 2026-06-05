using Melanin.API.DTO.Mappers;
using Melanin.API.DTO.Request;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Melanin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CartItemController : ControllerBase
{
    private readonly ICartItemService _cartItemService;

    public CartItemController(ICartItemService cartItemService)
    {
        _cartItemService = cartItemService;
    }

    [HttpGet("{memberId}")]
    [EndpointSummary("Récupérer le panier d'un membre")]
    public async Task<IActionResult> GetCart(int memberId)
    {
        IEnumerable<CartItem> cartItems = await _cartItemService.GetCartByMemberIdAsync(memberId);
        return Ok(cartItems.Select(ci => ci.ToDto()));
    }

    [HttpPost("{memberId}")]
    [EndpointSummary("Ajouter un produit au panier")]
    public async Task<IActionResult> AddToCart(int memberId, [FromBody] AddToCartDTO dto)
    {
        try
        {
            await _cartItemService.AddToCartAsync(memberId, dto.ProductId, dto.Quantity);
            return Ok(new { Message = "Produit ajouté au panier !" });
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

    [HttpPut("{cartItemId}")]
    [EndpointSummary("Modifier la quantité d'un article")]
    public async Task<IActionResult> UpdateQuantity(int cartItemId, [FromBody] UpdateCartItemDTO dto)
    {
        try
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _cartItemService.UpdateQuantityAsync(cartItemId, memberId, dto.Quantity);
            return Ok(new { Message = "Quantité mise à jour !" });
        }
        catch (CartItemNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpDelete("{cartItemId}")]
    [EndpointSummary("Supprimer un article du panier")]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        try
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _cartItemService.RemoveFromCartAsync(cartItemId, memberId);
            return Ok(new { Message = "Article supprimé du panier !" });
        }
        catch (CartItemNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpDelete("clear/{memberId}")]
    [EndpointSummary("Vider le panier d'un membre")]
    public async Task<IActionResult> ClearCart(int memberId)
    {
        await _cartItemService.ClearCartAsync(memberId);
        return Ok(new { Message = "Panier vidé !" });
    }
}