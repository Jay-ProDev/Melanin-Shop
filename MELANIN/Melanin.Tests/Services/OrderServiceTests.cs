using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Utils;
using Melanin.Application.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using Melanin.Tests.TestHelpers;
using Moq;

namespace Melanin.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task ShipAsync_OrderDoesNotExist_ShouldThrowOrderNotFoundException()
        {
            // ===================================================================
            // MOCKING — les 5 dépendances du constructeur d'OrderService
            // (une seule sera configurée ici ; les autres bouchent le constructeur)
            // ===================================================================
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // La commande n'existe pas → le repo renvoie null
            mockOrderRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync((Order?)null);

            OrderService service = new OrderService(
                mockOrderRepository.Object,
                mockAddressRepository.Object,
                mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT & ASSERT
            // ===================================================================
            await Assert.ThrowsAsync<OrderNotFoundException>(
                () => service.ShipAsync(1)
            );
        }


        [Fact]
        public async Task ShipAsync_OrderExists_ShouldUpdateOrderAndSendEmail()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // 1. Créer le membre qui recevra l'email
            Member member = new Member(
                firstName: "Alice",
                lastName: "Martin",
                email: "alice@test.com",
                passwordHash: "hashed"
            );

            // 2. Créer la commande
            Order order = new Order(memberId: 99, shippingAddressId: 44);

            // 3. La commande doit être CONFIRMED pour pouvoir être expédiée
            //    (Ship() exige ce statut ; un Order neuf est Pending)
            TestHelper.SetProperty(order, "Status", OrderStatus.Confirmed);

            // 4. La commande doit avoir un Member (ShipAsync lit order.Member.Email)
            TestHelper.SetProperty(order, "Member", member);

            // 5. Donner un Id à la commande (utilisé dans l'email)
            TestHelper.SetId(order, 7);

            // Le repo trouve la commande
            mockOrderRepository
                .Setup(repo => repo.GetByIdAsync(7))
                .ReturnsAsync(order);

            OrderService service = new OrderService(
                mockOrderRepository.Object,
                mockAddressRepository.Object,
                mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT
            // ===================================================================
            await service.ShipAsync(7);

            // ===================================================================
            // ASSERT — deux comportements à vérifier
            // ===================================================================

            // a) La commande a bien été sauvegardée
            mockOrderRepository.Verify(repo => repo.UpdateAsync(order), Times.Once);

            // b) L'email d'expédition a bien été envoyé au bon membre
            mockMailerUtil.Verify(
                mailer => mailer.SendOrderShippedEmailAsync("alice@test.com", "Alice", 7),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateAsync_CartIsEmpty_ShouldThrowEmptyCartException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Le panier du membre est vide → GetByMemberIdAsync renvoie une liste vide
            mockCartItemRepository
                .Setup(repo => repo.GetByMemberIdAsync(99))
                .ReturnsAsync(new List<CartItem>());

            OrderService service = new OrderService(
                mockOrderRepository.Object,
                mockAddressRepository.Object,
                mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT & ASSERT
            // ===================================================================
            await Assert.ThrowsAsync<EmptyCartException>(
                () => service.CreateAsync(memberId: 99, shippingAddressId: 1)
            );
        }

        [Fact]
        public async Task CreateAsync_AddressBelongsToAnotherMember_ShouldThrowAddressNotFoundException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Le panier contient au moins un article → on passe l'étape 1 (panier non vide)
            CartItem cartItem = new CartItem(
                quantity: 1,
                unitPrice: 50m,
                productId: 1,
                memberId: 99
            );

            mockCartItemRepository
                .Setup(repo => repo.GetByMemberIdAsync(99))
                .ReturnsAsync(new List<CartItem> { cartItem });

            // L'adresse existe MAIS appartient à un AUTRE membre (5, pas 99) → IDOR
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

            OrderService service = new OrderService(
                mockOrderRepository.Object,
                mockAddressRepository.Object,
                mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT & ASSERT
            // Le membre 99 tente de commander avec l'adresse du membre 5 → refusé
            // ===================================================================
            await Assert.ThrowsAsync<AddressNotFoundException>(
                () => service.CreateAsync(memberId: 99, shippingAddressId: 1)
            );
        }

        [Fact]
        public async Task CreateAsync_ProductOutOfStock_ShouldThrowProductOutOfStockException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Le panier contient un article : 5 unités du produit 1
            CartItem cartItem = new CartItem(
                quantity: 5,
                unitPrice: 50m,
                productId: 1,
                memberId: 99
            );

            mockCartItemRepository
                .Setup(repo => repo.GetByMemberIdAsync(99))
                .ReturnsAsync(new List<CartItem> { cartItem });

            // L'adresse est valide et appartient bien au membre 99 (on passe l'étape 2)
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

            // Le produit n'a que 4 en stock, alors qu'on en demande 5 → stock insuffisant
            Product product = new Product(
                name: "Perruque Lisse",
                description: "Une belle perruque",
                unitPrice: 50m,
                stockQuantity: 4,
                categoryId: 1
            );

            mockProductRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            OrderService service = new OrderService(
                mockOrderRepository.Object,
                mockAddressRepository.Object,
                mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT & ASSERT
            // On demande 5 unités alors que le stock est de 4 → exception
            // ===================================================================
            await Assert.ThrowsAsync<ProductOutOfStockException>(
                () => service.CreateAsync(memberId: 99, shippingAddressId: 1)
            );
        }

        [Fact]
        public async Task CreateAsync_ValidCart_ShouldDecreaseStockSaveOrderAndClearCart()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IOrderRepository> mockOrderRepository = new Mock<IOrderRepository>();
            Mock<IAddressRepository> mockAddressRepository = new Mock<IAddressRepository>();
            Mock<ICartItemRepository> mockCartItemRepository = new Mock<ICartItemRepository>();
            Mock<IProductRepository> mockProductRepository = new Mock<IProductRepository>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Panier : 2 unités du produit 1
            CartItem cartItem = new CartItem(
                quantity: 2,
                unitPrice: 50m,
                productId: 1,
                memberId: 99
            );

            mockCartItemRepository
                .Setup(repo => repo.GetByMemberIdAsync(99))
                .ReturnsAsync(new List<CartItem> { cartItem });

            // Adresse valide appartenant au membre 99
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

            // Produit avec assez de stock (10 ≥ 2)
            Product product = new Product(
                name: "Perruque Lisse",
                description: "Une belle perruque",
                unitPrice: 50m,
                stockQuantity: 10,
                categoryId: 1
            );

            mockProductRepository
                .Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Snapshot d'adresse : on lui donne un Id valide (> 0) quand on l'enregistre
            // (sinon new Order(..., savedSnapshot.Id, ...) lèverait une exception car Id = 0)
            mockAddressRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Address>()))
                .ReturnsAsync((Address addressToSave) =>
                {
                    TestHelper.SetId(addressToSave, 1);
                    return addressToSave;
                });

            // L'order créé dans le service : on le renvoie tel quel
            mockOrderRepository
                .Setup(repo => repo.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order orderToSave) => orderToSave);

            OrderService service = new OrderService(
                mockOrderRepository.Object,
                mockAddressRepository.Object,
                mockCartItemRepository.Object,
                mockProductRepository.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT
            // ===================================================================
            Order result = await service.CreateAsync(memberId: 99, shippingAddressId: 1);

            // ===================================================================
            // ASSERT — les trois effets du succès
            // ===================================================================

            // La méthode renvoie bien une commande
            Assert.NotNull(result);

            // Effet 1 : le stock du produit a été mis à jour (décrémenté)
            mockProductRepository.Verify(
                repo => repo.UpdateAsync(product),
                Times.Once
            );

            // Effet 2 : la commande a bien été sauvegardée
            mockOrderRepository.Verify(
                repo => repo.AddAsync(It.IsAny<Order>()),
                Times.Once
            );

            // Effet 3 : le panier du membre a été vidé
            mockCartItemRepository.Verify(
                repo => repo.ClearCartAsync(99),
                Times.Once
            );
        }
    }
}