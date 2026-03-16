using devhouse.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatatabaseConfig(builder.Configuration)
                .AddSwaggerGen()
                .AddServices()
                .AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello world");

app.UseSwagger().UseSwaggerUI();

app.MapControllers();

app.Run();