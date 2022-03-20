using AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers;
using AuthenticationService.Infrastructure.PasswordHashers;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Infrastructure.PasswordHashers
{
    public static class PasswordHashersServiceExtension
    {
        public static IServiceCollection AddBCryptPasswordHasher(
            this IServiceCollection services
        )
        {
            services.AddTransient<IPasswordHasher, BCryptPasswordHasher>();
            
            return services;
        }
    }
}