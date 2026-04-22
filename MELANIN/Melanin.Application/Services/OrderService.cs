using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Melanin.Domain.Enums;

namespace Melanin.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IAddressRepository addressRepository,
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _addressRepository = addressRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        // ===== Lecture =====

        public async Task<Order> GetByIdAsync(int id)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null)
                throw new OrderNotFoundException(id);
            return order;
        }

        public async Task<Order> GetByIdForMemberAsync(int id, int memberId)
        {
            Order? order = await _orderRepository.GetByIdAsync(id);
            if (order is null || order.MemberId != memberId)
                throw new OrderNotFoundException(id);
            return order;
        }

        public async Task<IEnumerable<Order>> GetByMemberIdAsync(int memberId)
        {
            return await _orderRepository.GetByMemberIdAsync(memberId);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
        {
            return await _orderRepository.GetByStatusAsync(status);
        }

        // ===== Transitions de statut =====

        public async Task ConfirmAsync(int orderId)
        {
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new OrderNotFoundException(orderId);

            order.Confirm();

            await _orderRepository.UpdateAsync(order);
        }

        public async Task ShipAsync(int orderId)
        {
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new OrderNotFoundException(orderId);

            order.Ship();

            await _orderRepository.UpdateAsync(order);
        }

        public async Task DeliverAsync(int orderId)
        {
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new OrderNotFoundException(orderId);

            order.Deliver();

            await _orderRepository.UpdateAsync(order);
        }

        public async Task CancelByAdminAsync(int orderId)
        {
            Order? order = await _orderRepository.GetByIdAsync(orderId);
            if (order is null)
                throw new OrderNotFoundException(orderId);

            // Restaurer le stock pour chaque produit commandé
            foreach (OrderItem item in order.OrderItems)
            {
                Product? product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product is not null)
                {
                    product.IncreaseStock(item.Quantity);
                    await _productRepository.UpdateAsync(product);
                }
            }

            order.Cancel();
            await _orderRepository.UpdateAsync(order);
        }

        public async Task CancelByClientAsync(int orderId, int memberId)
        {
            // GetByIdForMemberAsync gère déjà la vérification d'existence + IDOR
            Order order = await GetByIdForMemberAsync(orderId, memberId);

            // Restaurer le stock pour chaque produit commandé
            foreach (OrderItem item in order.OrderItems)
            {
                Product? product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product is not null)
                {
                    product.IncreaseStock(item.Quantity);
                    await _productRepository.UpdateAsync(product);
                }
            }

            order.Cancel();
            await _orderRepository.UpdateAsync(order);
        }

        // ===== Création =====

        public async Task<Order> CreateAsync(int memberId, int shippingAddressId, int? billingAddressId = null, string? couponCode = null)
        {
            // 1. Récupérer le panier du membre
            IEnumerable<CartItem> cartItems = await _cartItemRepository.GetByMemberIdAsync(memberId);
            if (!cartItems.Any())
                throw new EmptyCartException();

            // 2. Vérifier l'adresse de livraison (existence + propriété IDOR)
            Address? address = await _addressRepository.GetByIdAsync(shippingAddressId);
            if (address is null || address.MemberId != memberId)
                throw new AddressNotFoundException(shippingAddressId);

            // 3. Vérifier tous les stocks AVANT toute modification (atomicité)
            foreach (CartItem item in cartItems)
            {
                Product? product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product is null)
                    throw new ProductNotFoundException(item.ProductId);
                if (product.StockQuantity < item.Quantity)
                    throw new ProductOutOfStockException(product.Name);
            }

            // 4. Créer le snapshot de l'adresse (copie pour préserver l'historique)
            Address snapshot = new Address(
                address.City,
                address.PostalCode,
                address.Country,
                address.Street,
                address.Phone,
                memberId,
                address.FullName
            );
            Address savedSnapshot = await _addressRepository.AddAsync(snapshot);

            // 5. Créer l'Order avec l'id du snapshot
            Order order = new Order(
                memberId,
                savedSnapshot.Id,
                billingAddressId,
                couponCode
            );

            // 6. Pour chaque CartItem : décrémenter le stock + créer un OrderItem
            foreach (CartItem item in cartItems)
            {
                Product? product = await _productRepository.GetByIdAsync(item.ProductId);
                product!.DecreaseStock(item.Quantity);
                await _productRepository.UpdateAsync(product);

                OrderItem orderItem = new OrderItem(
                    item.Quantity,
                    item.UnitPrice,
                    item.ProductId
                );
                order.AddItem(orderItem);
            }

            // 7. Sauvegarder l'Order 
            Order savedOrder = await _orderRepository.AddAsync(order);

            // 8. Vider le panier du membre
            await _cartItemRepository.ClearCartAsync(memberId);

            return savedOrder;
        }
    }
}