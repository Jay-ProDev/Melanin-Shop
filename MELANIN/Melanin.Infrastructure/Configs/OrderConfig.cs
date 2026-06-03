using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melanin.Infrastructure.Database.Configs;

internal class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        // Nom de la table en base
        builder.ToTable("Order");

        // Clé primaire
        builder.HasKey(o => o.Id)
            .HasName("PK_Order")
            .IsClustered();

        // Colonnes
        builder.Property(o => o.Id)
            .HasColumnName("Id_Order")
            .ValueGeneratedOnAdd();

        builder.Property(o => o.TotalPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(o => o.CreatedAt)
            .HasColumnType("datetime");

        // Enum stocké en string
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasSentinel(0)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(o => o.CouponCode)
            .HasMaxLength(50);

        // Clés étrangères
        builder.Property(o => o.MemberId)
            .HasColumnName("Id_Member");

        builder.Property(o => o.ShippingAddressId)
            .HasColumnName("Id_ShippingAddress");

        builder.Property(o => o.BillingAddressId)
            .HasColumnName("Id_BillingAddress");

        // Relations

        // Order a plusieurs OrderItems (cascade : OrderItem n'existe pas sans Order)
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .HasConstraintName("FK_OrderItem__Order")
            .OnDelete(DeleteBehavior.Cascade);

        // Order a une ShippingAddress obligatoire (restrict : préserver l'historique)
        builder.HasOne(o => o.ShippingAddress)
            .WithMany()
            .HasForeignKey(o => o.ShippingAddressId)
            .HasConstraintName("FK_Order__ShippingAddress")
            .OnDelete(DeleteBehavior.Restrict);

        // Order a une BillingAddress optionnelle (restrict : préserver l'historique)
        builder.HasOne(o => o.BillingAddress)
            .WithMany()
            .HasForeignKey(o => o.BillingAddressId)
            .HasConstraintName("FK_Order__BillingAddress")
            .OnDelete(DeleteBehavior.Restrict);

        // Order a plusieurs Payments (restrict : préserver l'historique financier)
        // Tentatives échouées, paiement en N fois, remboursements
        builder.HasMany(o => o.Payments)
            .WithOne(p => p.Order)
            .HasForeignKey(p => p.OrderId)
            .HasConstraintName("FK_Payment__Order")
            .OnDelete(DeleteBehavior.Restrict);
    }
}