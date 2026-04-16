using Melanin.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Melanin.Infrastructure.Database.Configs;

internal class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Address");

        builder.HasKey(a => a.Id)
            .HasName("PK_Address")
            .IsClustered();

        builder.Property(a => a.Id)
            .HasColumnName("Id_Address")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.FullName)
            .HasMaxLength(100);
    }
}