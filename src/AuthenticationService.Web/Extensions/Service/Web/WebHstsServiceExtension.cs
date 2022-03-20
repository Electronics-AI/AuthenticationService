using System;
using AuthenticationService.Web.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Web
{
    public static class WebHstsServiceExtension
    {
        public static IServiceCollection AddWebHstsWithHttpsRedirection(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            WebHstsConfiguration webHstsConfiguration = new WebHstsConfiguration();
            configuration.Bind("Hsts", webHstsConfiguration);

            services.AddHsts(options =>
            {
                options.Preload = webHstsConfiguration.Preload;
                options.IncludeSubDomains = webHstsConfiguration.IncludeSubDomains;
                options.MaxAge = TimeSpan.FromDays(webHstsConfiguration.MaxAgeDays);
                foreach (string host in webHstsConfiguration.ExcludedHosts) {
                    options.ExcludedHosts.Add(host);
                }
            });

            services.AddHttpsRedirection(options =>
            {
                // 307 - Temporary Redirect, 308 - Permanent Redirect
                options.RedirectStatusCode = webHstsConfiguration.HttpsRedirectionStatusCode;
                options.HttpsPort = webHstsConfiguration.HttpsPort;
            });
    
            return services;
        }
    }
}