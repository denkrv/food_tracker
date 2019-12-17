using Microsoft.Extensions.DependencyInjection;

namespace Food.API
{
    public static class CustomExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "FoodTracker - Food.API",
                    Version = "v1",
                    Description = "The Food Microservice HTTP API.",
                    License = new Microsoft.OpenApi.Models.OpenApiLicense()
                    {
                        Name = "MIT"
                    }
                });
            });

            return services;

        }
    }
}