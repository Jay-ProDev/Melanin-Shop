using Melanin.Application.Interfaces.Repositories;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Melanin.Infrastructure.Database.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MelaninDbContext _dbContext;

        public MemberRepository(MelaninDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Member?> GetByIdAsync(int id)
        {
            return await _dbContext.Members.FindAsync(id);
        }

        public async Task<Member?> GetByEmailAsync(string email)
        {
            return await _dbContext.Members
                .FirstOrDefaultAsync(m => m.Email == email);
        }

        public async Task<string?> GetHashPwdAsync(string email)
        {
            return await _dbContext.Members
                .Where(m => m.Email == email)
                .Select(m => m.PasswordHash)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _dbContext.Members.ToListAsync();
        }

        public async Task<Member> AddAsync(Member member)
        {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
            return member;
        }

        public async Task UpdateAsync(Member member)
        {
            _dbContext.Members.Update(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            Member? member = await _dbContext.Members.FindAsync(id);
            _dbContext.Members.Remove(member!);
            await _dbContext.SaveChangesAsync();
        }
    }
}
