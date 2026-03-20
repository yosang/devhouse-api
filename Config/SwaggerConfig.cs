using System.Reflection;
using Microsoft.OpenApi.Models;
namespace devhouse.Extensions;

public static class SwaggerConfig
{
    /// <summary>Adds Swagger Gen configuration, including OpenApi documentation, inline XML comments for endpoint descriptions and Bearer authentication</summary>
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

            // Allow swagger to read XML comments, enabled on .csproj and found in BasePath/bin/debug/net9.0/devhouse.xml
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

            // Defines what a Bearer token scheme should look like, 
            // This enables the visual Authentication button on swagger docs
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            // This is new scheme that references to our previous "Bearer" scheme
            var bearerScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
            };

            // Adds the required scheme for any endpoint that requires Authorization
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { bearerScheme, new List<string>() }
            });
        }

        );

        return service;
    }

    /// <summary>Enables Swagger middlewares for development environment only</summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseSwaggerConfig(this WebApplication app)
    {
        // Swagger is currently only available to development during API testing.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}