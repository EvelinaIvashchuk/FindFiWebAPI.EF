using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class PricingConfiguration : IEntityTypeConfiguration<Pricing>
{
    public void Configure(EntityTypeBuilder<Pricing> builder)
    {
        builder.ToTable("Pricing");

        builder.HasKey(x => x.ListingId);

        builder.Property(x => x.PricePerMonth)
            .HasColumnType("decimal(12,2)");

        builder.Property(x => x.PricePerNight)
            .HasColumnType("decimal(12,2)");

        builder.Property(x => x.Currency)
            .HasMaxLength(3)
            .HasColumnType("char(3)")
            .HasDefaultValue("UAH")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}
