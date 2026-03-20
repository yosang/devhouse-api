using devhouse.Services;

namespace devhouse.Extensions;

public static class ServicesConfig
{
    /// <summary>Adds various services to the DI container requested by controllers</summary>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
                .AddScoped<ProjectTypeService>()
                .AddScoped<DeveloperService>()
                .AddScoped<ProjectService>()
                .AddScoped<RoleService>()
                .AddScoped<TeamService>()
                .AddScoped<AuthService>();

        return services;
    }
}