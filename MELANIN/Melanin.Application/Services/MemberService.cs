using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Application.Interfaces.Utils;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Soenneker.Hashing.Argon2;


namespace Melanin.Application.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IMailerUtil _mailerUtil;

        public MemberService(IMemberRepository memberRepository, IMailerUtil mailerUtil)
        {
            _memberRepository = memberRepository;
            _mailerUtil = mailerUtil;
        }

        public async Task<Member> RegisterAsync(Member member)
        {
            // Vérifier si l'email existe déjà
            Member? existing = await _memberRepository.GetByEmailAsync(member.Email);
            if (existing is not null)
                throw new MemberAlreadyExistsException();

            // Hasher le mot de passe
            Member memberToRegister = new Member(
                firstName: member.FirstName,
                lastName: member.LastName,
                email: member.Email,
                passwordHash: await Argon2HashingUtil.Hash(member.PasswordHash)
            );

            // Convertir en entité et sauvegarder
            Member registered = await _memberRepository.AddAsync(memberToRegister);
            await _mailerUtil.SendWelcomeEmailAsync(registered.Email, registered.FirstName);
            return registered;
        }


        public async Task<Member> LoginAsync(string email, string password)
        {
            string? hashPwd = await _memberRepository.GetHashPwdAsync(email);
            if (hashPwd is null)
                throw new MemberBadCredentialException();

            bool isValid = await Argon2HashingUtil.Verify(password, hashPwd);
            if (!isValid)
                throw new MemberBadCredentialException();

            Member? member = await _memberRepository.GetByEmailAsync(email);

            return member!;
        }


        public async Task<Member> GetByIdAsync(int id)
        {
            Member? member = await _memberRepository.GetByIdAsync(id);
            if (member is null)
                throw new MemberNotFoundException(id);
            return member;
        }

        public async Task<IEnumerable<Member>> GetAllAsync()
        {
            return await _memberRepository.GetAllAsync();
        }

        public async Task UpdateAsync(Member member)
        {
            Member? memberInDB = await _memberRepository.GetByIdAsync(member.Id);
            if (memberInDB is null)
                throw new MemberNotFoundException();

            await _memberRepository.UpdateAsync(member);
        }

        public async Task DeleteAsync(int id)
        {
            Member? member = await _memberRepository.GetByIdAsync(id);
            if (member is null)
                throw new MemberNotFoundException();

            await _memberRepository.DeleteAsync(id);
        }
    }
}
