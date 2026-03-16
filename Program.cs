var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();


app.UseHttpsRedirection();


app.MapGet("/weatherforecast", () => "Hello world");

app.Run();

