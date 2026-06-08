using Melanin.API.DTO.Mappers;
using Melanin.API.DTO.Request;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melanin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Créer un produit")]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
    {
        try
        {
            Product created = await _productService.CreateAsync(dto.ToEntity());
            return Ok(created.ToDto());
        }
        catch (CategoryNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpGet("{id}")]
    [EndpointSummary("Récupérer un produit par Id")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            Product product = await _productService.GetByIdAsync(id);
            return Ok(product.ToDto());
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpGet]
    [EndpointSummary("Récupérer tous les produits (Admin)")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Product> products = await _productService.GetAllAsync();
        return Ok(products.Select(p => p.ToDto()));
    }

    [HttpGet("active")]
    [EndpointSummary("Récupérer tous les produits actifs (Shop)")]
    public async Task<IActionResult> GetAllActive()
    {
        IEnumerable<Product> products = await _productService.GetAllActiveAsync();
        return Ok(products.Select(p => p.ToDto()));
    }

    [HttpGet("category/{categoryId}")]
    [EndpointSummary("Récupérer les produits par catégorie")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        IEnumerable<Product> products = await _productService.GetByCategoryIdAsync(categoryId);
        return Ok(products.Select(p => p.ToDto()));
    }

    [HttpGet("search")]
    [EndpointSummary("Rechercher des produits")]
    public async Task<IActionResult> Search([FromQuery] string keyword)
    {
        IEnumerable<Product> products = await _productService.SearchAsync(keyword);
        return Ok(products.Select(p => p.ToDto()));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Modifier un produit")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDTO dto)
    {
        try
        {
            Product product = await _productService.GetByIdAsync(id);
            product.Update(dto.Name, dto.Description, dto.UnitPrice, dto.CategoryId, dto.HairColor, dto.HairLength, dto.HairTexture, dto.CapSize);
            await _productService.UpdateAsync(product);
            return Ok(new { Message = "Produit mis à jour !" });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPut("{id}/activate")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Activer un produit")]
    public async Task<IActionResult> Activate(int id)
    {
        try
        {
            Product product = await _productService.GetByIdAsync(id);
            product.Activate();
            await _productService.UpdateAsync(product);
            return Ok(new { Message = "Produit activé !" });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPut("{id}/deactivate")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Désactiver un produit")]
    public async Task<IActionResult> Deactivate(int id)
    {
        try
        {
            Product product = await _productService.GetByIdAsync(id);
            product.Deactivate();
            await _productService.UpdateAsync(product);
            return Ok(new { Message = "Produit désactivé !" });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPost("{id}/image")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Upload une image pour un produit")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        try
        {
            if (file is null || file.Length == 0)
                return BadRequest(new { Message = "Aucun fichier envoyé." });

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest(new { Message = "Le fichier ne doit pas dépasser 5 Mo." });

            string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest(new { Message = "Format non supporté (jpg, jpeg, png, webp uniquement)." });

            Product product = await _productService.GetByIdAsync(id);

            // Supprimer l'ancienne image si elle existe
            if (!string.IsNullOrEmpty(product.ImageUrl))
            {
                string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(oldImagePath))
                    System.IO.File.Delete(oldImagePath);
            }

            // Sauvegarder le nouveau fichier
            string imageName = $"{id}_{Guid.NewGuid()}{fileExtension}";
            string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");

            if (!Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            string imagePath = Path.Combine(imageFolder, imageName);
            using (FileStream stream = new FileStream(imagePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            product.SetImageUrl($"/images/products/{imageName}");
            await _productService.UpdateAsync(product);

            return Ok(new { Message = "Image uploadée avec succès", ImageUrl = product.ImageUrl });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPut("{id}/stock/increase")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Réapprovisionner le stock")]
    public async Task<IActionResult> IncreaseStock(int id, [FromBody] UpdateStockDTO dto)
    {
        try
        {
            Product product = await _productService.GetByIdAsync(id);
            product.IncreaseStock(dto.Quantity);
            await _productService.UpdateAsync(product);
            return Ok(new { Message = "Stock mis à jour !" });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpPut("{id}/stock/decrease")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Diminuer le stock manuellement")]
    public async Task<IActionResult> DecreaseStock(int id, [FromBody] UpdateStockDTO dto)
    {
        try
        {
            Product product = await _productService.GetByIdAsync(id);
            product.DecreaseStock(dto.Quantity);
            await _productService.UpdateAsync(product);
            return Ok(new { Message = "Stock mis à jour !" });
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

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer un produit")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productService.DeleteAsync(id);
            return Ok(new { Message = "Produit supprimé !" });
        }
        catch (ProductNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}