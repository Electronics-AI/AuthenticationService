using System.Reflection;
using System.Text.Json.Serialization;
using AuthenticationService.API.Controllers;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.API
{
    public static class ApiControllersServiceExtension
    {
        public static IServiceCollection AddApiControllers(
            this IServiceCollection services
        )
        {   
            var apiAssembly = Assembly.GetAssembly(typeof(AuthenticationController));

            services
                .AddControllers()
                .AddApplicationPart(apiAssembly)
                .AddControllersAsServices()
                .AddFluentValidation();
            
            return services;
        }

        public static IMvcBuilder AddJsonModules(this IMvcBuilder builder)
        {
            builder.AddJsonOptions(options => 
                        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                    .AddNewtonsoftJson();

            return builder;
        }
    }
}