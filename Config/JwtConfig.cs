namespace devhouse.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using devhouse.jwt;
using devhouse.Services;

public static class JwtConfig
{
    /// <summary>Adds JWT configurations to the DI container as a singleton using settings from appsettings.json</summary>
    /// <param name="service"></param>
    /// <param name="options"></param>
    /// <returns>Microsoft.Extensions.DependencyInjection.IServiceCollection</returns>
    public static IServiceCollection AddJwtConfig(this IServiceCollection service, IConfiguration options)
    {
        var jwtSettings = options.GetRequiredSection("JwtSettings").Get<JwtSettings>()!;

        service.AddSingleton(jwtSettings)
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = jwtSettings.tokenValidationParameters);


        // TokenService is responsible for generating tokens, as well as accessing token claims through current HttpContext
        // We are using it as a singleton, just like jwtSettings, as neither depend on database context
        service.AddSingleton<TokenService>().AddHttpContextAccessor();

        // Adds default authorization policies, which allows us to restrict endpoints with the [Authorize] attribute
        service.AddAuthorization();

        return service;
    }

    /// <summary>Enables Authentication and Authorization middlewares</summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseAuthConfig(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}