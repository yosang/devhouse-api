namespace devhouse.Extensions;

public static class CorsConfig
{
    public static IServiceCollection AddCorsConfig(this IServiceCollection service)
    {
        service.AddCors(options =>
        {
            options.AddPolicy("Development", policy =>
            {
                policy.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
            });

            options.AddPolicy("Production", policy =>
            {
                policy.AllowAnyMethod()
                     .AllowAnyHeader()
                     .WithOrigins(
                         "https://myfrontend.com",
                         "https://admin.myfrontend.com"
                     );
            });
        });

        return service;
    }

    public static WebApplication UseCorsConfig(this WebApplication app)
    {
        string env = app.Environment.IsDevelopment() ? "Development" : "Production";
        app.UseCors(env);

        app.Logger.LogInformation($"Using CORS policy: {env}");

        return app;
    }
}