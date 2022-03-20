using AuthenticationService.Web.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Web
{
    public static class WebResponseCachingServiceExtension
    {
        public static IServiceCollection AddWebResponseCaching(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var webResponseCachingConfiguration = new WebResponseCachingConfiguration();
            configuration.Bind("ResponseCaching", webResponseCachingConfiguration);

            services.AddResponseCaching(options => 
            {
                options.MaximumBodySize = webResponseCachingConfiguration.MaximumBodySizeBytes;
                options.SizeLimit = webResponseCachingConfiguration.CacheSizeLimitBytes;
                options.UseCaseSensitivePaths = webResponseCachingConfiguration.UseCaseSensitivePaths;
            });

            return services;
        }
    }
}