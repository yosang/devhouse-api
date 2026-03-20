using devhouse.Extensions;
using devhouse.Services;

// var auth = new AuthService(null, null);
// Console.WriteLine(auth.HashedPassword("admin1234"));
// Console.WriteLine(auth.HashedPassword("lead1234"));
// Console.WriteLine(auth.HashedPassword("developer1234"));

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