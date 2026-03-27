using Melanin.Domain.Entities;


namespace Melanin.Application.Interfaces.Services;

public interface IMemberService
{
    Task<Member> RegisterAsync(Member member);
    Task<Member> LoginAsync(string email, string password);
    Task<Member> GetByIdAsync(int id);
    Task<IEnumerable<Member>> GetAllAsync();
    Task UpdateAsync(Member member);
    Task DeleteAsync(int id);
}
