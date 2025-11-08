using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(64);
        builder.HasIndex(x => x.Code).IsUnique().HasDatabaseName("UQ_Product_Code");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
        builder.HasIndex(x => x.Name).HasDatabaseName("IX_Product_Name");

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.Property(x => x.Price)
            .HasColumnType("decimal(12,2)");
        builder.HasCheckConstraint("CK_Product_Price_Positive", "`Price` >= 0");

        builder.Property(x => x.IsActive).HasDefaultValue(true);
        builder.HasIndex(x => x.IsActive).HasDatabaseName("IX_Product_IsActive");

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime(3)")
            .HasDefaultValueSql("current_timestamp(3)");

        builder.HasMany(x => x.OrderItems)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_OrderItem_Product");
    }
}