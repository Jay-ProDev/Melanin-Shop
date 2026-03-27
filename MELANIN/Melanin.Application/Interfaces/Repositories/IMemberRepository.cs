using Melanin.Domain.Entities;


namespace Melanin.Application.Interfaces.Repositories;

public interface IMemberRepository
{   
    Task<Member?> GetByIdAsync(int id);
    Task<Member?> GetByEmailAsync(string email);
    Task<string?> GetHashPwdAsync(string email);
    Task<IEnumerable<Member>> GetAllAsync();
    Task<Member> AddAsync(Member member);
    Task UpdateAsync(Member member);
    Task DeleteAsync(int id);
}
