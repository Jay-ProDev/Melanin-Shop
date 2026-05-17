using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melanin.Infrastructure.Database.Configs;

internal class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        // Nom de la table en base
        builder.ToTable("Payment");

        // Clé primaire
        builder.HasKey(p => p.Id)
            .HasName("PK_Payment")
            .IsClustered();

        // Colonnes
        builder.Property(p => p.Id)
            .HasColumnName("Id_Payment")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Amount)
            .IsRequired()
            .HasPrecision(10, 2);

        // Enum stocké en string (cohérent avec OrderStatus)
        builder.Property(p => p.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.PaidAt)
            .HasColumnType("datetime");

        builder.Property(p => p.StripeSessionId)
            .IsRequired()
            .HasMaxLength(255);

        // Index unique sur StripeSessionId : lookup rapide depuis le webhook
        builder.HasIndex(p => p.StripeSessionId)
            .IsUnique()
            .HasDatabaseName("IDX_Payment__stripeSessionId");   

        // Clés étrangères
        builder.Property(p => p.OrderId)
            .HasColumnName("Id_Order");

        // Relation Payment → Order :
        // déclarée côté Order (HasMany Payments) dans OrderConfiguration,
        // pas re-déclarée ici pour éviter les conflits EF Core
    }
}