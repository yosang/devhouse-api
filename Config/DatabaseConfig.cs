using devhouse.Context;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Extensions;

public static class DatabaseConfig
{

    public static IServiceCollection AddDatatabaseConfig(this IServiceCollection service, IConfiguration options)
    {
        var conStr = options.GetConnectionString("Default")!;
        service.AddDbContext<DatabaseContext>(options => options.UseMySQL(conStr));
        return service;
    }
}