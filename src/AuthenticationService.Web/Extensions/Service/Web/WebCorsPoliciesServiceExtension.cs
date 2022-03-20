using AuthenticationService.Web.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Web
{
    public static class WebCorsServiceExtension
    {
        public static IServiceCollection AddWebCorsPolicies(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            WebCorsConfiguration webCorsConfiguration = new WebCorsConfiguration();
            configuration.Bind("Cors", webCorsConfiguration);

            services.AddCors(options => 
            {
                options.AddDefaultPolicy(
                    builder => builder.WithOrigins(webCorsConfiguration.AllowedOrigins.ToArray())
                );
            });

            return services;
        }
    }
}