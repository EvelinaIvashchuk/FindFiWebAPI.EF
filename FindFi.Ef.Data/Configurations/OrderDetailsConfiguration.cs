using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetails>
{
    public void Configure(EntityTypeBuilder<OrderDetails> builder)
    {
        builder.ToTable("OrderDetails");
        builder.HasKey(x => x.OrderId);

        builder.Property(x => x.PaymentMethod)
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.TransactionId).HasMaxLength(100);
        builder.Property(x => x.BillingEmail).HasMaxLength(256);
        builder.Property(x => x.BillingAddress).HasMaxLength(400);
        builder.Property(x => x.Notes).HasMaxLength(1000);

        builder.HasOne(x => x.Order)
            .WithOne(x => x.Details)
            .HasForeignKey<OrderDetails>(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_OrderDetails_Order");
    }
}