using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melanin.Infrastructure.Database.Configs;

internal class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItem");

        builder.HasKey(ci => ci.Id)
               .HasName("PK_CartItem")
               .IsClustered();

        builder.Property(ci => ci.Id)
               .HasColumnName("Id_CartItem")
               .ValueGeneratedOnAdd();

        builder.Property(ci => ci.Quantity)
               .IsRequired();

        builder.Property(ci => ci.UnitPrice)
               .HasPrecision(10, 2)
               .IsRequired();

        builder.Property(ci => ci.ProductId)
               .HasColumnName("Id_Product");

        builder.Property(ci => ci.MemberId)
               .HasColumnName("Id_Member");

        // Relation Product → CartItems (Product n'a pas de collection)
        builder.HasOne(ci => ci.Product)
               .WithMany()
               .HasForeignKey(ci => ci.ProductId)
               .HasConstraintName("FK_CartItem__Product")
               .OnDelete(DeleteBehavior.Restrict);
    }
}