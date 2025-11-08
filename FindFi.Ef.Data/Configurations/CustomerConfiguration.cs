using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customer");
        builder.HasKey(x => x.CustomerId);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(256);
        builder.HasIndex(x => x.Email).IsUnique().HasDatabaseName("UQ_Customer_Email");

        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(200);
        builder.HasIndex(x => x.FullName).HasDatabaseName("IX_Customer_FullName");

        builder.Property(x => x.Phone)
            .HasMaxLength(32);

        builder.Property(x => x.Role)
            .HasDefaultValue((byte)0);

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime(3)")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(3)");
        builder.HasIndex(x => x.CreatedAt).HasDatabaseName("IX_Customer_CreatedAt");

        builder.HasMany(x => x.Addresses)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_CustomerAddress_Customer");

        builder.HasMany(x => x.Orders)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Order_Customer");
    }
}