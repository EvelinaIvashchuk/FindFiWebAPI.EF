using FindFi.Ef.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FindFi.Ef.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<Media> Media => Set<Media>();
    public DbSet<Pricing> Pricing => Set<Pricing>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Availability> Availabilities => Set<Availability>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Listing>().ToTable("Listing");
        modelBuilder.Entity<Media>().ToTable("Media");
        modelBuilder.Entity<Pricing>().ToTable("Pricing");
        modelBuilder.Entity<Tag>().ToTable("Tag");
        modelBuilder.Entity<Availability>().ToTable("Availability");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}