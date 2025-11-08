using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class CustomerAddressConfiguration : IEntityTypeConfiguration<CustomerAddress>
{
    public void Configure(EntityTypeBuilder<CustomerAddress> builder)
    {
        builder.ToTable("CustomerAddress");
        builder.HasKey(x => x.AddressId);

        builder.Property(x => x.Line1).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Line2).HasMaxLength(200);
        builder.Property(x => x.City).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Region).HasMaxLength(100);
        builder.Property(x => x.Country).IsRequired().HasMaxLength(100);
        builder.Property(x => x.PostalCode).HasMaxLength(20);
        builder.Property(x => x.IsPrimary).HasDefaultValue(false);
        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime(3)")
            .HasDefaultValueSql("current_timestamp(3)");

        builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_CustomerAddress_CustomerId");

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Addresses)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CustomerAddress_Customer");
    }
}