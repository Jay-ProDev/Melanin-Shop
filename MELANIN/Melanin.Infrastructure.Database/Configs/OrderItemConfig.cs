using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melanin.Infrastructure.Database.Configs;

internal class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem");

        builder.HasKey(oi => oi.Id)
            .HasName("PK_OrderItem")
            .IsClustered();

        builder.Property(oi => oi.Id)
            .HasColumnName("Id_OrderItem")
            .ValueGeneratedOnAdd();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(oi => oi.OrderId)
            .HasColumnName("Id_Order");

        builder.Property(oi => oi.ProductId)
            .HasColumnName("Id_Product");

        // Relation OrderItem → Product (Product n'a pas de collection)
        // Restrict : protéger l'historique comptable des commandes
        builder.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .HasConstraintName("FK_OrderItem__Product")
            .OnDelete(DeleteBehavior.Restrict);

        // Relation OrderItem → Order définie dans OrderConfiguration (un seul sens)
    }
}