using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Melanin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    // ===== ENDPOINT CLIENT =====

    [HttpPost("checkout-session/{orderId}")]
    [EndpointSummary("Créer une session de paiement Stripe pour une commande")]
    public async Task<IActionResult> CreateCheckoutSession(int orderId)
    {
        try
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            string url = await _paymentService.CreateCheckoutSessionAsync(orderId, memberId);
            return Ok(new { Url = url });
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

    // ===== ENDPOINT STRIPE (WEBHOOK) =====

    [HttpPost("webhook")]
    [AllowAnonymous]
    [EndpointSummary("Réception des événements de paiement Stripe")]
    public async Task<IActionResult> Webhook()
    {
        try
        {
            // Lecture du corps brut de la requête (nécessaire pour la vérification de signature)
            using StreamReader reader = new StreamReader(Request.Body);
            string payload = await reader.ReadToEndAsync();

            // Récupération de la signature envoyée par Stripe dans les en-têtes
            string signature = Request.Headers["Stripe-Signature"]!;

            await _paymentService.HandleWebhookAsync(payload, signature);

            return Ok();
        }
        catch (PaymentWebhookException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (PaymentNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (InvalidOrderStatusException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}