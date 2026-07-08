using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Utils;
using Melanin.Application.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Moq;

namespace Melanin.Tests.Services
{
    public class MemberServiceTests
    {
        [Fact]
        public async Task RegisterAsync_EmailAlreadyExists_ShouldThrowMemberAlreadyExistsException()
        {
            // ===================================================================
            // MOCKING — Simulation des dépendances
            // On crée de faux objets à la place du vrai repository et du vrai mailer.
            // Ils n'iront jamais en base de données ni n'enverront d'email réel.
            // ===================================================================
            Mock<IMemberRepository> mockRepository = new Mock<IMemberRepository>();
            Mock<IMailerUtil> mockMailer = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE — Préparation
            // On configure le comportement du faux repository, on crée le service
            // à tester, et on prépare les données d'entrée.
            // ===================================================================

            // Un membre qui existe "déjà" dans la fausse base
            Member existingMember = new Member(
                firstName: "Jay",
                lastName: "Test",
                email: "jay@test.com",
                passwordHash: "hashed"
            );

            // On dit au faux repo : "si on te demande cet email, renvoie ce membre"
            mockRepository
                .Setup(repo => repo.GetByEmailAsync("jay@test.com"))
                .ReturnsAsync(existingMember);

            // On crée le vrai service, mais avec les FAUX objets injectés
            MemberService service = new MemberService(mockRepository.Object, mockMailer.Object);

            // Les données qu'on va tenter d'inscrire (même email → conflit attendu)
            Member newMember = new Member(
                firstName: "Jay",
                lastName: "Test",
                email: "jay@test.com",
                passwordHash: "password123"
            );

            // ===================================================================
            // ACT & ASSERT — Action et Vérification
            // Ici l'action et la vérification sont combinées : on appelle la méthode
            // ET on vérifie qu'elle lance bien l'exception attendue.
            // ===================================================================
            await Assert.ThrowsAsync<MemberAlreadyExistsException>(
                () => service.RegisterAsync(newMember)
            );
        }


        [Fact]
        public async Task RegisterAsync_EmailIsFree_ShouldReturnRegisteredMember()
        {
            // ===================================================================
            // MOCKING — Simulation des dépendances
            // ===================================================================
            Mock<IMemberRepository> mockRepository = new Mock<IMemberRepository>();
            Mock<IMailerUtil> mockMailer = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE — Préparation
            // ===================================================================

            // 1. L'email est libre → le repo renvoie null quand on le cherche
            mockRepository
                .Setup(repo => repo.GetByEmailAsync("nouveau@test.com"))
                .ReturnsAsync((Member?)null);

            // 2. Quand on sauvegarde, le repo renvoie le membre qu'on lui a donné
            mockRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Member>()))
                .ReturnsAsync((Member memberPassed) => memberPassed);

            // On crée le service avec les faux objets
            MemberService service = new MemberService(mockRepository.Object, mockMailer.Object);

            // Les données du nouveau membre à inscrire
            Member newMember = new Member(
                firstName: "Alice",
                lastName: "Martin",
                email: "nouveau@test.com",
                passwordHash: "monMotDePasse"
            );

            // ===================================================================
            // ACT — Action
            // On exécute la méthode qu'on veut tester
            // ===================================================================
            Member result = await service.RegisterAsync(newMember);

            // ===================================================================
            // ASSERT — Vérification
            // On vérifie que le résultat correspond à ce qu'on attend
            // ===================================================================
            Assert.NotNull(result);
            Assert.Equal("Alice", result.FirstName);
            Assert.Equal("nouveau@test.com", result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_MemberDoesNotExist_ShouldThrowMemberNotFoundException()
        {
            // ===================================================================
            // MOCKING — Simulation des dépendances
            // ===================================================================
            Mock<IMemberRepository> mockRepository = new Mock<IMemberRepository>();
            Mock<IMailerUtil> mockMailer = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE — Préparation
            // ===================================================================

            // Le membre n'existe pas → le repo renvoie null
            mockRepository
                .Setup(repo => repo.GetByIdAsync(99))
                .ReturnsAsync((Member?)null);

            MemberService service = new MemberService(mockRepository.Object, mockMailer.Object);

            // ===================================================================
            // ACT & ASSERT — l'exception est attendue
            // ===================================================================
            await Assert.ThrowsAsync<MemberNotFoundException>(
                () => service.GetByIdAsync(99)
            );
        }


        [Fact]
        public async Task GetByIdAsync_MemberExists_ShouldReturnMember()
        {
            // ===================================================================
            // MOCKING — Simulation des dépendances
            // ===================================================================
            Mock<IMemberRepository> mockRepository = new Mock<IMemberRepository>();
            Mock<IMailerUtil> mockMailer = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE — Préparation
            // ===================================================================

            // Le membre existe → le repo le renvoie quand on cherche l'id 1
            Member existingMember = new Member(
                firstName: "Alice",
                lastName: "Martin",
                email: "alice@test.com",
                passwordHash: "hashed"
            );

            mockRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(existingMember);

            MemberService service = new MemberService(mockRepository.Object, mockMailer.Object);

            // ===================================================================
            // ACT — Action
            // ===================================================================
            Member result = await service.GetByIdAsync(1);

            // ===================================================================
            // ASSERT — Vérification
            // ===================================================================
            Assert.NotNull(result);
            Assert.Equal("alice@test.com", result.Email);
        }
    }
}