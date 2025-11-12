using FindFi.Ef.Data;

namespace FindFi.Ef.Api.Infrastructure;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db, CancellationToken cancellationToken = default)
    {
        // No-op seeding for the new schema. Intentionally left blank.
        await Task.CompletedTask;
    }
}
