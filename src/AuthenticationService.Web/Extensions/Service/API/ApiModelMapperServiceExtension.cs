using System.Reflection;
using AuthenticationService.API.Models.Requests.Authentication;
using AuthenticationService.Web;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.API
{
    public static class DtoMappingServiceExtension 
    {
        public static IServiceCollection AddApiModelMapper(
            this IServiceCollection services
        )
        {
            var apiAssembly = Assembly.GetAssembly(typeof(LoginUserRequestDto));
            
            services.AddAutoMapper(apiAssembly);

            return services;
        }
    }
}