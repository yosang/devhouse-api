using devhouse.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatatabaseConfig(builder.Configuration)
                .AddSwaggerConfig()
                .AddServices()
                .AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello world");

app.UseSwaggerMiddlewares();

app.MapControllers();

app.Run();