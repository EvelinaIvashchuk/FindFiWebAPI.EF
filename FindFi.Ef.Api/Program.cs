using FindFi.Ef.Bll;
using FindFi.Ef.Data;
using FindFi.Ef.Api.Middleware;
using FindFi.Ef.Api.Infrastructure;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Allow env var overrides
builder.Configuration.AddEnvironmentVariables();

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console());

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FindFi.Ef API",
        Version = "v1",
        Description = "EF Core Code‑First REST API для пошуку та оренди квартир/будинків без рієлторів. Містить приклад (Listings, Tags) з DTO/AutoMapper/BLL/UoW.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "FindFi.EF", 
            Url = new Uri("https://example.com")
        }
    });
});

// FluentValidation (no validators currently)
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AppDbContext>();

var connectionString = builder.Configuration.GetConnectionString("DB1")
                       ?? builder.Configuration.GetConnectionString("Default")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DB1")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__Default")
                       ?? string.Empty;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

builder.Services.AddEfDal();
builder.Services.AddEfBll();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Request logging + Correlation
app.UseSerilogRequestLogging();
app.UseMiddleware<CorrelationIdMiddleware>();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

// Ensure DB and seed (idempotent)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    await db.Database.MigrateAsync();
    var seedingEnabled = builder.Configuration.GetValue<bool?>("Seeding:Enabled")
                         ?? app.Environment.IsDevelopment();
    if (false)
    {
        await DbSeeder.SeedAsync(db);
    }
}

app.Run();