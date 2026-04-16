using Melanin.API.DTO.Request;
using Melanin.API.DTO.Response;
using Melanin.Domain.Entities;

namespace Melanin.API.DTO.Mappers;

internal static class MemberMapper
{
    // Member → MemberResponseDto
    public static MemberResponseDTO ToDto(this Member member)
    {
        return new MemberResponseDTO
        {
            Id = member.Id,
            FirstName = member.FirstName,
            LastName = member.LastName,
            Email = member.Email,
            CreatedAt = member.CreatedAt
        };
    }

    // RegisterDTO → Member
    public static Member ToEntity(this RegisterDTO dto)
    {
        return new Member(
            dto.FirstName,
            dto.LastName,
            dto.Email,
            dto.Password
        );
    }

    //ToEntity → vers la requête(client envoie des données → on crée en base)
    //ToDto → vers la réponse(on lit en base → on renvoie au client)
}