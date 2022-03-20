using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace AuthenticationService.Web.Extensions.Host
{
    public static class SerilogHostExtension
    {
        public static IHostBuilder ConfigureAndUseSerilog(
            this IHostBuilder hostBuilder
        )
        {
            hostBuilder.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Services(services)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.File(
                    path: context.Configuration["Serilog:LogFilePath"],
                    outputTemplate: "{Timestamp:G}: {Message}{NewLine:1}{Exception:1}",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    rollingInterval: RollingInterval.Day
                    )
                .WriteTo.Seq(
                    serverUrl: context.Configuration["Serilog:SeqServerUrl"],
                    restrictedToMinimumLevel: LogEventLevel.Information
                )
                );
            
            return hostBuilder;
        }
    }
}