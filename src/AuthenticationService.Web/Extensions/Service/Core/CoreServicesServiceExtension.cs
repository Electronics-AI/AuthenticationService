using AuthenticationService.Core.Interfaces.Services;
using AuthenticationService.Core.Services.Authentication;
using AuthenticationService.Core.Services.Token;
using AuthenticationService.Core.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Core
{
    public static class TokenServiceExtension
    {
        public static IServiceCollection AddCoreServices(
            this IServiceCollection services
        )
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            
            return services;
        }
    }
}