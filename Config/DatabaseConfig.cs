using devhouse.Context;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Extensions;

public static class DatabaseConfig
{

    /// <summary>Adds database context to the DI container as scoped with MySQL connection string from appsettings.json</summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    /// <returns>Microsoft.Extensions.DependencyInjection.IServiceCollection</returns>
    public static IServiceCollection AddDatabaseConfig(this IServiceCollection service, IConfiguration configuration)
    {
        var conStr = configuration.GetConnectionString("Default")!;
        service.AddDbContext<DatabaseContext>(options => options.UseMySQL(conStr));
        return service;
    }
}