using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AuthenticationService.Web.Extensions.Host
{
    public static class AppConfigurationHostExtension
    {
        public static IHostBuilder ConfigureAndUseAppConfiguration(
            this IHostBuilder hostBuilder
        )
        {
            hostBuilder.ConfigureAppConfiguration((hostingContext, config) => 
                {
                    string envName = hostingContext.HostingEnvironment.EnvironmentName;
                    
                    config.Sources.Clear();

                    config
                        .AddJsonFile($"appsettings.{envName}.json")
                        .AddEnvironmentVariables(prefix: "AuthenticationService_");
                });

            return hostBuilder;
        }
    }
}