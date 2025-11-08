using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItem");
        builder.HasKey(x => x.OrderItemId);

        builder.Property(x => x.UnitPrice)
            .HasColumnType("decimal(12,2)");
        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.Property(x => x.LineTotal)
            .HasColumnType("decimal(14,2)")
            .HasComputedColumnSql("(`UnitPrice` * `Quantity`)", stored: true);

        builder.HasIndex(x => x.OrderId).HasDatabaseName("IX_OrderItem_OrderId");
        builder.HasIndex(x => x.ProductId).HasDatabaseName("IX_OrderItem_ProductId");
        builder.HasIndex(x => new { x.OrderId, x.ProductId }).IsUnique().HasDatabaseName("UQ_OrderItem_Order_Product");

        builder.HasCheckConstraint("CK_OrderItem_Quantity_Positive", "`Quantity` > 0");
        builder.HasCheckConstraint("CK_OrderItem_UnitPrice_Positive", "`UnitPrice` >= 0");

        builder.HasOne(x => x.Order)
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderItem_Order");

        builder.HasOne(x => x.Product)
            .WithMany(x => x.OrderItems)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_OrderItem_Product");
    }
}