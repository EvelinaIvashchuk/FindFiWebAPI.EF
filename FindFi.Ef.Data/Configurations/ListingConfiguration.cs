using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FindFi.Ef.Data.Configurations;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.ToTable("Listing");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasColumnType("text");

        // enum('apartment','house','room') stored as lowercase text
        var propertyTypeConverter = new ValueConverter<PropertyType, string>(
            v => v == PropertyType.Apartment ? "apartment" : (v == PropertyType.House ? "house" : "room"),
            v => v == "apartment" ? PropertyType.Apartment : (v == "house" ? PropertyType.House : PropertyType.Room));

        builder.Property(x => x.PropertyType)
            .HasConversion(propertyTypeConverter)
            .HasMaxLength(16)
            .HasColumnType("enum('apartment','house','room')")
            .IsRequired();

        var listingTypeConverter = new ValueConverter<ListingType, string>(
            v => v == ListingType.LongTerm ? "long_term" : "short_term",
            v => v == "long_term" ? ListingType.LongTerm : ListingType.ShortTerm);

        builder.Property(x => x.ListingType)
            .HasConversion(listingTypeConverter)
            .HasMaxLength(16)
            .HasColumnType("enum('long_term','short_term')")
            .IsRequired();

        builder.Property(x => x.City)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.AddressLine)
            .HasMaxLength(250);

        builder.Property(x => x.Latitude)
            .HasColumnType("decimal(9,6)");
        builder.Property(x => x.Longitude)
            .HasColumnType("decimal(9,6)");

        builder.Property(x => x.AreaSqM)
            .HasColumnType("decimal(8,2)");

        builder.Property(x => x.PetsAllowed)
            .HasDefaultValue(false);

        builder.Property(x => x.DefaultCurrency)
            .HasMaxLength(3)
            .HasColumnType("char(3)")
            .HasDefaultValue("UAH")
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("datetime")
            .ValueGeneratedOnAddOrUpdate();

        // Indexes
        builder.HasIndex(x => x.City).HasDatabaseName("IX_Listing_City");
        builder.HasIndex(x => x.HostId).HasDatabaseName("IX_Listing_Host");
        builder.HasIndex(x => new { x.PropertyType, x.ListingType }).HasDatabaseName("IX_Listing_Type");

        // Relations
        builder.HasOne(l => l.Pricing)
            .WithOne(p => p.Listing)
            .HasForeignKey<Pricing>(p => p.ListingId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Pricing_Listing");

        builder.HasMany(l => l.Media)
            .WithOne(m => m.Listing)
            .HasForeignKey(m => m.ListingId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Media_Listing");

        builder.HasMany(l => l.Availabilities)
            .WithOne(a => a.Listing)
            .HasForeignKey(a => a.ListingId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_Availability_Listing");

        builder
            .HasMany(l => l.Tags)
            .WithMany(t => t.Listings)
            .UsingEntity<ListingTag>(
                je => je
                    .HasOne(x => x.Tag)
                    .WithMany()
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.Cascade),
                je => je
                    .HasOne(x => x.Listing)
                    .WithMany()
                    .HasForeignKey(x => x.ListingId)
                    .OnDelete(DeleteBehavior.Cascade),
                je =>
                {
                    je.ToTable("ListingTag");
                    je.HasKey(x => new { x.ListingId, x.TagId });
                });
    }
}
