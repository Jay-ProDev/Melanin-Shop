using Melanin.Application.Interfaces.Repositories;
using Melanin.Application.Interfaces.Services;
using Melanin.Application.Interfaces.Utils;
using Melanin.Application.Services;
using Melanin.Domain.BusinessExecptions;
using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using Melanin.Tests.TestHelpers;
using Moq;

namespace Melanin.Tests.Services
{
    public class PaymentServiceTests
    {
        [Fact]
        public async Task HandleWebhookAsync_PaymentNotCompleted_ShouldDoNothing()
        {
            // ===================================================================
            // MOCKING — les 4 dépendances du constructeur de PaymentService
            // ===================================================================
            Mock<IPaymentRepository> mockPaymentRepository = new Mock<IPaymentRepository>();
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            Mock<IStripeUtil> mockStripeUtil = new Mock<IStripeUtil>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Stripe indique que le paiement N'EST PAS complété
            // (par ex. un autre type d'event que "checkout.session.completed")
            mockStripeUtil
                .Setup(stripe => stripe.VerifyAndParseWebhook("payload", "signature"))
                .Returns((isPaymentCompleted: false, sessionId: string.Empty));

            PaymentService service = new PaymentService(
                mockPaymentRepository.Object,
                mockOrderService.Object,
                mockStripeUtil.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT
            // ===================================================================
            await service.HandleWebhookAsync("payload", "signature");

            // ===================================================================
            // ASSERT — RIEN ne doit se passer après le return
            // ===================================================================

            // On n'a pas cherché de Payment
            mockPaymentRepository.Verify(
                repo => repo.GetByStripeSessionIdAsync(It.IsAny<string>()),
                Times.Never
            );

            // On n'a confirmé aucune commande
            mockOrderService.Verify(
                orderService => orderService.ConfirmAsync(It.IsAny<int>()),
                Times.Never
            );

            // On n'a envoyé aucun email
            mockMailerUtil.Verify(
                mailer => mailer.SendOrderConfirmedEmailAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()),
                Times.Never
            );
        }

        [Fact]
        public async Task HandleWebhookAsync_PaymentNotFound_ShouldThrowPaymentNotFoundException()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IPaymentRepository> mockPaymentRepository = new Mock<IPaymentRepository>();
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            Mock<IStripeUtil> mockStripeUtil = new Mock<IStripeUtil>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Stripe dit : paiement réussi pour la session "sess_123"
            mockStripeUtil
                .Setup(stripe => stripe.VerifyAndParseWebhook("payload", "signature"))
                .Returns((isPaymentCompleted: true, sessionId: "sess_123"));

            // Mais aucun Payment ne correspond à cette session → null
            mockPaymentRepository
                .Setup(repo => repo.GetByStripeSessionIdAsync("sess_123"))
                .ReturnsAsync((Payment?)null);

            PaymentService service = new PaymentService(
                mockPaymentRepository.Object,
                mockOrderService.Object,
                mockStripeUtil.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT & ASSERT
            // ===================================================================
            await Assert.ThrowsAsync<PaymentNotFoundException>(
                () => service.HandleWebhookAsync("payload", "signature")
            );
        }

        [Fact]
        public async Task HandleWebhookAsync_PaymentAlreadyCompleted_ShouldDoNothing()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IPaymentRepository> mockPaymentRepository = new Mock<IPaymentRepository>();
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            Mock<IStripeUtil> mockStripeUtil = new Mock<IStripeUtil>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Stripe dit : paiement réussi
            mockStripeUtil
                .Setup(stripe => stripe.VerifyAndParseWebhook("payload", "signature"))
                .Returns((isPaymentCompleted: true, sessionId: "sess_123"));

            // Le Payment existe mais est DÉJÀ complété
            Payment payment = new Payment(orderId: 1, amount: 50m, stripeSessionId: "sess_123");
            // On force le statut à Completed (on ne peut pas appeler MarkAsCompleted()
            // deux fois : la 2e lèverait une exception car statut != Pending)
            TestHelper.SetProperty(payment, "Status", PaymentStatus.Completed);

            mockPaymentRepository
                .Setup(repo => repo.GetByStripeSessionIdAsync("sess_123"))
                .ReturnsAsync(payment);

            PaymentService service = new PaymentService(
                mockPaymentRepository.Object,
                mockOrderService.Object,
                mockStripeUtil.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT
            // ===================================================================
            await service.HandleWebhookAsync("payload", "signature");

            // ===================================================================
            // ASSERT — le webhook doublon ne doit RIEN refaire
            // ===================================================================

            // Pas de nouvelle sauvegarde du Payment
            mockPaymentRepository.Verify(
                repo => repo.UpdateAsync(It.IsAny<Payment>()),
                Times.Never
            );

            // Pas de reconfirmation de commande
            mockOrderService.Verify(
                orderService => orderService.ConfirmAsync(It.IsAny<int>()),
                Times.Never
            );

            // Pas de 2e email
            mockMailerUtil.Verify(
                mailer => mailer.SendOrderConfirmedEmailAsync(
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<decimal>()),
                Times.Never
            );
        }

        [Fact]
        public async Task HandleWebhookAsync_PaymentPending_ShouldCompletePaymentConfirmOrderAndSendEmail()
        {
            // ===================================================================
            // MOCKING
            // ===================================================================
            Mock<IPaymentRepository> mockPaymentRepository = new Mock<IPaymentRepository>();
            Mock<IOrderService> mockOrderService = new Mock<IOrderService>();
            Mock<IStripeUtil> mockStripeUtil = new Mock<IStripeUtil>();
            Mock<IMailerUtil> mockMailerUtil = new Mock<IMailerUtil>();

            // ===================================================================
            // ARRANGE
            // ===================================================================

            // Stripe dit : paiement réussi
            mockStripeUtil
                .Setup(stripe => stripe.VerifyAndParseWebhook("payload", "signature"))
                .Returns((isPaymentCompleted: true, sessionId: "sess_123"));

            // Le Payment existe et est en attente (Pending par défaut à la création)
            Payment payment = new Payment(orderId: 1, amount: 50m, stripeSessionId: "sess_123");

            mockPaymentRepository
                .Setup(repo => repo.GetByStripeSessionIdAsync("sess_123"))
                .ReturnsAsync(payment);

            // À la fin, le service récupère l'Order pour envoyer l'email → il faut un Member dedans
            Member member = new Member(
                firstName: "Alice",
                lastName: "Martin",
                email: "alice@test.com",
                passwordHash: "hashed"
            );

            Order order = new Order(memberId: 99, shippingAddressId: 1);
            TestHelper.SetProperty(order, "Member", member);
            TestHelper.SetProperty(order, "TotalPrice", 50m);
            TestHelper.SetId(order, 1);

            mockOrderService
                .Setup(orderService => orderService.GetByIdAsync(1))
                .ReturnsAsync(order);

            PaymentService service = new PaymentService(
                mockPaymentRepository.Object,
                mockOrderService.Object,
                mockStripeUtil.Object,
                mockMailerUtil.Object
            );

            // ===================================================================
            // ACT
            // ===================================================================
            await service.HandleWebhookAsync("payload", "signature");

            // ===================================================================
            // ASSERT — les 3 effets doivent avoir eu lieu
            // ===================================================================

            // a) Le Payment a été sauvegardé (maintenant Completed)
            mockPaymentRepository.Verify(
                repo => repo.UpdateAsync(payment),
                Times.Once
            );

            // b) La commande a été confirmée
            mockOrderService.Verify(
                orderService => orderService.ConfirmAsync(1),
                Times.Once
            );

            // c) L'email de confirmation a été envoyé au bon membre
            mockMailerUtil.Verify(
                mailer => mailer.SendOrderConfirmedEmailAsync("alice@test.com", "Alice", 1, 50m),
                Times.Once
            );
        }
    }
}