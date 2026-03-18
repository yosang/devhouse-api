using devhouse.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatatabaseConfig(builder.Configuration)
                .AddSwaggerConfig()
                .AddJwtConfig(builder.Configuration)
                .AddServices()
                .AddControllers();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapGet("/", () => "Hello world");


app.UseSwaggerConfig()
    .UseAuthConfig()
    // Add Cors middleware
    .MapControllers();

app.Run();