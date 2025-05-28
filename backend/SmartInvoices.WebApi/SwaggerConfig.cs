using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace SmartInvoices.WebApi
{
    /// <summary>
    /// Konfiguracja Swagger dla aplikacji
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Dodaje usługi Swagger do aplikacji
        /// </summary>
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.SwaggerDocument(options =>
            {
                // Upewnij się, że wszystkie wersje API są uwzględnione
                options.MaxEndpointVersion = 1;

                // Uproszczenie nazw schematów
                options.ShortSchemaNames = true;

                // Konfiguracja dokumentu OpenAPI
                options.DocumentSettings = s =>
                {
                    s.Title = "SmartInvoices API";
                    s.Version = "v1";
                    s.Description = "API systemu zarządzania fakturami SmartInvoices";

                    // Konfiguracja autoryzacji JWT
                    s.AddAuth(
                        "JWT",
                        new()
                        {
                            Type = NSwag.OpenApiSecuritySchemeType.Http,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            Description = "Wprowadź token JWT bez przedrostka Bearer:"
                        }
                    );
                };

                // Włącz autoryzację JWT w Swagger UI
                options.EnableJWTBearerAuth = true;
            });

            return services;
        }

        /// <summary>
        /// Konfiguruje middleware Swagger w aplikacji
        /// </summary>
        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            return app.UseSwaggerGen();
        }
    }
}
