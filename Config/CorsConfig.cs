namespace devhouse.Extensions;

public static class CorsConfig
{
    const string DEVELOPMENT = "Development";
    const string PRODUCTION = "Production";

    /// <summary>Configures CORS policies for development and production environments</summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    /// <returns>Microsoft.Extensions.DependencyInjection.IServiceCollection</returns>
    public static IServiceCollection AddCorsConfig(this IServiceCollection service, IConfiguration configuration)
    {
        var origins = configuration.GetSection("AllowedOrigins").Get<string[]>() ?? new string[] { "https://myfrontend.com", "https://admin.myfrontend.com" };

        service.AddCors(options =>
        {
            options.AddPolicy(DEVELOPMENT, policy =>
            {
                policy.AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
            });

            options.AddPolicy(PRODUCTION, policy =>
            {
                policy.AllowAnyMethod()
                     .AllowAnyHeader()
                     .WithOrigins(origins);
            });
        });

        return service;
    }

    /// <summary>Enables CORS for the current environment</summary>
    /// <param name="app"></param>
    /// <returns>Microsoft.AspNetCore.Builder.WebApplication</returns>
    public static WebApplication UseCorsConfig(this WebApplication app)
    {
        string env = app.Environment.IsDevelopment() ? DEVELOPMENT : PRODUCTION;

        app.UseCors(env);
        app.Logger.LogInformation($"Using CORS policy: {env}");

        return app;
    }
}