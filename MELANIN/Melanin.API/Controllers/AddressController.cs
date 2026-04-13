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
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet]
    [EndpointSummary("Récupérer l'adresse du membre connecté")]
    public async Task<IActionResult> GetByMember()
    {
        int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Address? address = await _addressService.GetByMemberIdAsync(memberId);

        if (address is null)
            return NotFound(new { Message = "Aucune adresse enregistrée." });

        return Ok(address.ToDto());
    }

    [HttpPost]
    [EndpointSummary("Créer une adresse")]
    public async Task<IActionResult> Create([FromBody] CreateAddressDTO dto)
    {
        try
        {
            int memberId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            Address address = dto.ToEntity(memberId);
            await _addressService.AddAsync(address);
            return Ok(new { Message = "Adresse enregistrée !" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPut("{id}")]
    [EndpointSummary("Modifier une adresse")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAddressDTO dto)
    {
        try
        {
            Address address = await _addressService.GetByIdAsync(id);
            address.Update(dto.City, dto.PostalCode, dto.Country, dto.Street, dto.Phone, dto.FullName);
            await _addressService.UpdateAsync(address);
            return Ok(new { Message = "Adresse mise à jour !" });
        }
        catch (AddressNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [EndpointSummary("Supprimer une adresse")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _addressService.DeleteAsync(id);
            return Ok(new { Message = "Adresse supprimée !" });
        }
        catch (AddressNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}