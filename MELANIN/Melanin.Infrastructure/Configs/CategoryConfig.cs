using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melanin.Infrastructure.Database.Configs;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        // Nom de la table en base
        builder.ToTable("Category");

        // Clé primaire
        builder.HasKey(c => c.Id)
            .HasName("PK_Category")
            .IsClustered();

        // Colonnes
        builder.Property(c => c.Id)
            .HasColumnName("Id_Category")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(50);

        // Slug unique (deux catégories ne peuvent pas avoir le même slug)
        builder.HasIndex(c => c.Slug)
            .IsUnique()
            .HasDatabaseName("IDX_Category__slug");

        // Relations : une Category a plusieurs Products
        builder.HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .HasConstraintName("FK_Product__Category")
            .OnDelete(DeleteBehavior.Restrict);
    }
}