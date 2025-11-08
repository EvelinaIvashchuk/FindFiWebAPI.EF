using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Order");
        builder.HasKey(x => x.OrderId);

        builder.Property(x => x.CustomerId).IsRequired();

        builder.Property(x => x.Status)
            .HasDefaultValue((byte)0);
        builder.HasIndex(x => x.Status).HasDatabaseName("IX_Order_Status");

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasColumnType("char(3)")
            .HasDefaultValue("USD");

        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(12,2)");
        builder.HasCheckConstraint("CK_Order_TotalAmount_Positive", "`TotalAmount` >= 0");

        builder.Property(x => x.PlacedAt)
            .HasColumnType("datetime(3)");
        builder.HasIndex(x => x.PlacedAt).HasDatabaseName("IX_Order_PlacedAt");

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp(3)")
            .HasDefaultValueSql("CURRENT_TIMESTAMP(3)");

        builder.HasIndex(x => x.CustomerId).HasDatabaseName("IX_Order_CustomerId");

        builder.HasOne(x => x.Customer)
            .WithMany(x => x.Orders)
            .HasForeignKey(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Order_Customer");

        builder.HasMany(x => x.Items)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderItem_Order");

        builder.HasOne(x => x.Details)
            .WithOne(x => x.Order)
            .HasForeignKey<OrderDetails>(d => d.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderDetails_Order");
    }
}