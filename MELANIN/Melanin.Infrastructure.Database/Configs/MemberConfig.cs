using Melanin.Domain.Entities;
using Melanin.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Melanin.Infrastructure.Database.Configs;

internal class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        // Nom de la table en base
        builder.ToTable("Members");

        // Clé primaire
        builder.HasKey(m => m.Id)
            .HasName("PK_Member")
            .IsClustered();

        // Colonnes
        builder.Property(m => m.Id)
            .HasColumnName("Id_Member")
            .ValueGeneratedOnAdd();

        builder.Property(m => m.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.Email)
            .IsRequired()
            .HasMaxLength(320);

        // Email unique (comme dans ton script SQL)
        builder.HasIndex(m => m.Email)
            .IsUnique()
            .HasDatabaseName("IDX_Member__email");

        builder.Property(m => m.PasswordHash)
            .IsRequired()
            .HasMaxLength(255);

        // On stocke l'enum en string pour que ce soit lisible en base
        builder.Property(m => m.Role)
            .HasConversion<string>()
            .HasDefaultValue(MemberRole.User)
            .HasSentinel(0)
            .HasMaxLength(50)
            .IsRequired();


        builder.Property(m => m.CreatedAt)
            .HasColumnType("datetime");

        // Relations : un Member a plusieurs Addresses
        builder.HasMany(m => m.Addresses)
            .WithOne(a => a.Member)
            .HasForeignKey(a => a.MemberId)
            .HasConstraintName("FK_Address__Member")
            .OnDelete(DeleteBehavior.Cascade);

        // Un Member a plusieurs CartItems
        builder.HasMany(m => m.CartItems)
            .WithOne(c => c.Member)
            .HasForeignKey(c => c.MemberId)
            .HasConstraintName("FK_CartItem__Member")
            .OnDelete(DeleteBehavior.Cascade);

        // Un Member a plusieurs Orders
        builder.HasMany(m => m.Orders)
            .WithOne(o => o.Member)
            .HasForeignKey(o => o.MemberId)
            .HasConstraintName("FK_Order__Member")
            .OnDelete(DeleteBehavior.Restrict);
    }
}