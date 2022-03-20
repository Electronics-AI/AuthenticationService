using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;

namespace AuthenticationService.Web.Extensions.Host
{
    public static class KestrelHostExtension
    {
        public static IHostBuilder ConfigureKestrelFromAppsettings(
            this IHostBuilder hostBuilder
        )
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.Configure<KestrelServerOptions>(
                    context.Configuration.GetSection("Kestrel"));
            });
            
            return hostBuilder;
        }

        public static IWebHostBuilder ConfigureAndUseKestrel(
            this IWebHostBuilder webhostBuilder
        )
        {
            webhostBuilder.UseKestrel((hostContext, serverOptions) => 
            {
                bool envIsProductionOrStaging =
                    hostContext.HostingEnvironment.IsProduction() || hostContext.HostingEnvironment.IsStaging();

                if (envIsProductionOrStaging) {
                    serverOptions.ListenAnyIP(5000);
                    serverOptions.ListenAnyIP(5001, listenOptions => {
                        listenOptions.UseHttps(
                            hostContext.Configuration["Ssl:CertificatePath"], 
                            hostContext.Configuration["Ssl:CertificatePassword"]);
                    });
                }
                else if (hostContext.HostingEnvironment.IsDevelopment()) {
                    serverOptions.ListenAnyIP(5000);
                }
            });

            return webhostBuilder;
        }
    }
}