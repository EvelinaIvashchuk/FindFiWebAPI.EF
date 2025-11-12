using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class AvailabilityConfiguration : IEntityTypeConfiguration<Availability>
{
    public void Configure(EntityTypeBuilder<Availability> builder)
    {
        builder.ToTable("Availability");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.DateFrom)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.DateTo)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(x => x.IsAvailable)
            .HasDefaultValue(true);

        builder.HasCheckConstraint("CK_Availability_Dates", "`DateFrom` < `DateTo`");

        builder.HasIndex(x => new { x.ListingId, x.DateFrom, x.DateTo })
            .HasDatabaseName("IX_Availability_Listing");
    }
}
