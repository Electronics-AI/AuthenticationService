using AuthenticationService.Infrastructure.Loggers.Serilog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Web.Extensions.Service.Infrastructure.Loggers
{
    public static class LoggersServiceExtension
    {
        public static IServiceCollection AddSerilogLogger(
            this IServiceCollection services
        ) 
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddScoped(typeof(AuthenticationService.Core.Interfaces.Infrastructure.Loggers.ILogger<>),
                               typeof(GenericSerilogLogger<>));
            return services;
        }
    }
}