using Melanin.API.DTO.Mappers;
using Melanin.API.DTO.Request;
using Melanin.API.Token;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Melanin.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly TokenTool _tokenTool;

    public MemberController(IMemberService memberService, TokenTool tokenTool)
    {
        _memberService = memberService;
        _tokenTool = tokenTool;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        try
        {
            await _memberService.RegisterAsync(dto.ToEntity());
            return Ok(new { Message = "Votre compte a bien été créé !" });
        }
        catch (MemberAlreadyExistsException ex)
        {
            return Conflict(new { ex.Message });
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        try
        {
            Member result = await _memberService.LoginAsync(dto.Email, dto.Password);

            string token = _tokenTool.Generate(new TokenTool.Data
            {
                MemberId = result.Id,
                Role = result.Role.ToString()
            });

            return Ok(new { Message = "Connexion réussie !", Token = token });
        }
        catch (MemberNotFoundException ex)
        {
            return Unauthorized(new { ex.Message });
        }
        catch (MemberBadCredentialException ex)
        {
            return Unauthorized(new { ex.Message });
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            Member result = await _memberService.GetByIdAsync(id);
            return Ok(result.ToDto());
        }
        catch (MemberNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Member> result = await _memberService.GetAllAsync();
        return Ok(result.Select(m => m.ToDto()));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateMemberDTO dto)
    {
        try
        {
            Member member = await _memberService.GetByIdAsync(id);
            member.Update(dto.FirstName, dto.LastName, dto.Email);
            await _memberService.UpdateAsync(member);

            return Ok(new { Message = "Membre mis à jour !" });
        }
        catch (MemberNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _memberService.DeleteAsync(id);
            return Ok(new { Message = "Membre supprimé !" });
        }
        catch (MemberNotFoundException ex)
        {
            return NotFound(new { ex.Message });
        }
    }
}