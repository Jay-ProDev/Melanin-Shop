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
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Créer une catégorie")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
    {
        try
        {
            Category created = await _categoryService.CreateAsync(dto.ToEntity());
            return Ok(created.ToDto());
        }
        catch (CategoryAlreadyExistsException ex)
        {
            return Conflict(new { ex.Message });
        }
    }

    [HttpGet("{id}")]
    [EndpointSummary("Récupérer une catégorie par Id")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            Category category = await _categoryService.GetByIdAsync(id);
            return Ok(category.ToDto());
        }
        catch (CategoryNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpGet("slug/{slug}")]
    [EndpointSummary("Récupérer une catégorie par Slug")]
    public async Task<IActionResult> GetBySlug(string slug)
    {
        try
        {
            Category category = await _categoryService.GetBySlugAsync(slug);
            return Ok(category.ToDto());
        }
        catch (CategoryNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpGet]
    [EndpointSummary("Récupérer toutes les catégories")]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Category> categories = await _categoryService.GetAllAsync();
        return Ok(categories.Select(c => c.ToDto()));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Modifier une catégorie")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryDTO dto)
    {
        try
        {
            Category category = await _categoryService.GetByIdAsync(id);
            category.Update(dto.Name, dto.Slug);
            await _categoryService.UpdateAsync(category);
            return Ok(new { Message = "Catégorie mise à jour !" });
        }
        catch (CategoryNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
        catch (CategoryAlreadyExistsException ex)
        {
            return Conflict(new { ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [EndpointSummary("Supprimer une catégorie")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _categoryService.DeleteAsync(id);
            return Ok(new { Message = "Catégorie supprimée !" });
        }
        catch (CategoryNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}