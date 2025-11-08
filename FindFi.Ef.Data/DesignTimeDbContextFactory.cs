using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace FindFi.Ef.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        var cs = config.GetConnectionString("DB1")
                 ?? config.GetConnectionString("Default")
                 ?? Environment.GetEnvironmentVariable("ConnectionStrings__DB1")
                 ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default");

        if (string.IsNullOrWhiteSpace(cs))
            throw new InvalidOperationException("Connection string not found. Set ConnectionStrings:DB1 or ConnectionStrings:Default or corresponding env vars.");

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(cs, ServerVersion.AutoDetect(cs)); 
        return new AppDbContext(optionsBuilder.Options);
    }
}