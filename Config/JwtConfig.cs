namespace devhouse.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using devhouse.jwt;

public static class JwtConfig
{
    public static IServiceCollection AddJwtConfig(this IServiceCollection service, IConfiguration options)
    {
        var jwtSettings = options.GetRequiredSection("JwtSettings").Get<JwtSettings>()!;

        service.AddSingleton(jwtSettings)
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => options.TokenValidationParameters = jwtSettings.tokenValidationParameters);

        service.AddAuthorization();

        return service;
    }

    public static WebApplication UseAuthConfig(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}