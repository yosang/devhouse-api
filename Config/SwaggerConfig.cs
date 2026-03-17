using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
namespace devhouse.Extensions;

public static class SwaggerConfig
{
    public static IServiceCollection AddSwaggerConfig(this IServiceCollection service)
    {
        service.AddSwaggerGen(options =>
        {

            // Adds OpenApi metadata to the Swagger generator
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Devhouse API",
                Description = "An ASP.NET Core API to manage Devhouse in-house development projects"
            });


            // options.IncludeXmlComments()
        }

        );

        return service;
    }

    public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}