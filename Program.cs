using devhouse.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabaseConfig(builder.Configuration)
                .AddSwaggerConfig()
                .AddJwtConfig(builder.Configuration)
                .AddCorsConfig()
                .AddServices()
                .AddControllers();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapGet("/", () => "Visit /swagger for API documentation");


app.UseSwaggerConfig()
    .UseCorsConfig()
    .UseAuthConfig()
    // Add HTTPS
    .MapControllers();

app.Run();