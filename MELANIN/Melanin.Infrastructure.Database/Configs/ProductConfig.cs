using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melanin.Infrastructure.Database.Configs;

internal class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Nom de la table en base
        builder.ToTable("Product");

        // Clé primaire
        builder.HasKey(p => p.Id)
            .HasName("PK_Product")
            .IsClustered();

        // Colonnes
        builder.Property(p => p.Id)
            .HasColumnName("Id_Product")
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.UnitPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(p => p.StockQuantity)
            .IsRequired();

        builder.Property(p => p.ImageUrl)
            .HasMaxLength(255);

        builder.Property(p => p.CreatedAt)
            .HasColumnType("datetime");

        builder.Property(p => p.IsActive)
            .IsRequired();

        builder.Property(p => p.HairColor)
            .HasMaxLength(50);

        // Enums nullable → stockés en string
        builder.Property(p => p.HairLength)
            .HasConversion<string>()
            .HasSentinel(0)
            .HasMaxLength(20);

        builder.Property(p => p.HairTexture)
            .HasConversion<string>()
            .HasSentinel(0)
            .HasMaxLength(20);

        builder.Property(p => p.CapSize)
            .HasConversion<string>()
            .HasSentinel(0)
            .HasMaxLength(20);
    }
}