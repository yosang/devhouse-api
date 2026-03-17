using devhouse.Services;

namespace devhouse.Extensions;

public static class ServicesConfig
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
                .AddScoped<ProjectTypeService>()
                .AddScoped<DeveloperService>()
                .AddScoped<ProjectService>()
                .AddScoped<RoleService>()
                .AddScoped<TeamService>()
                .AddSingleton<TokenService>();

        return services;
    }
}