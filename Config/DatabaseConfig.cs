using devhouse.Context;
using Microsoft.EntityFrameworkCore;

namespace devhouse.Extensions;

public static class DatabaseConfig
{
    public static string ConnectionString { get; private set; } = string.Empty;

    public static IServiceCollection AddDatatabaseConfig(this IServiceCollection service, IConfiguration options)
    {
        try
        {
            ConnectionString = options.GetRequiredSection("ConnectionString").Value!;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Unable to retrieve connection string: {ex.Message}");
            throw;
        }

        service.AddDbContext<DatabaseContext>(options => options.UseMySQL(ConnectionString));
        return service;
    }
}