using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FindFi.Ef.Data.Configurations;

public class MediaConfiguration : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.ToTable("Media");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.IsCover)
            .HasDefaultValue(false);

        builder.Property(x => x.SortOrder)
            .HasDefaultValue(0);

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetime")
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => new { x.ListingId, x.IsCover })
            .HasDatabaseName("IX_Media_Listing");
    }
}
