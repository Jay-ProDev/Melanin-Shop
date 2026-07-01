using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Moq;

namespace Melanin.Tests.Services
{
    public class AddressServiceTests
    {
        [Fact]
        public async Task GetByIdForMemberAsync_AddressDoesNotExist_ShouldThrowAddressNotFoundException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // L'adresse n'existe pas → le repo renvoie null
            mockAddressRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Address?)null);

            AddressService service = new AddressService(mockAddressRepository.Object);

            // ===================================================================
            // ACT & ASSERT
            // ===================================================================
            await Assert.ThrowsAsync<AddressNotFoundException>(
                () => service.GetByIdForMemberAsync(1, memberId: 99)
            );
        }


        [Fact]
        public async Task GetByIdForMemberAsync_AddressBelongsToAnotherMember_ShouldThrowAddressNotFoundException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // L'adresse existe et appartient au membre 5
            Address address = new Address(
                city: "Bruxelles",
                postalCode: "1000",
                country: "Belgique",
                street: "Rue Test 1",
                phone: "0470000000",
                memberId: 5
            );

            mockAddressRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(address);

            AddressService service = new AddressService(mockAddressRepository.Object);

            // ===================================================================
            // ACT & ASSERT
            // Le membre 99 tente d'accéder à l'adresse du membre 5 → refusé (IDOR)
            // On lance la MÊME exception que "inexistante" pour ne pas révéler
            // que l'adresse existe.
            // ===================================================================
            await Assert.ThrowsAsync<AddressNotFoundException>(
                () => service.GetByIdForMemberAsync(1, memberId: 99)
            );
        }


        [Fact]
        public async Task GetByIdForMemberAsync_AddressBelongsToMember_ShouldReturnAddress()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // L'adresse existe et appartient bien au membre 99
            Address address = new Address(
                city: "Bruxelles",
                postalCode: "1000",
                country: "Belgique",
                street: "Rue Test 1",
                phone: "0470000000",
                memberId: 99
            );

            mockAddressRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(address);

            AddressService service = new AddressService(mockAddressRepository.Object);

            // ===================================================================
            // ACT
            // ===================================================================
            Address result = await service.GetByIdForMemberAsync(1, memberId: 99);

            // ===================================================================
            // ASSERT
            // ===================================================================
            Assert.NotNull(result);
            Assert.Equal(99, result.MemberId);
            Assert.Equal("Bruxelles", result.City);
        }
    }
}