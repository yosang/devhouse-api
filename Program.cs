using devhouse.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfig(builder.Configuration)
                .AddSwaggerConfig()
                .AddJwtConfig(builder.Configuration)
                .AddCorsConfig(builder.Configuration)
                .AddServices()
                .AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Visit /swagger for API documentation");

// With Hsts, HTTPS is strictly enforced on production environments
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSwaggerConfig()
    .UseCorsConfig()
    .UseAuthConfig()
    .MapControllers();

app.Run();