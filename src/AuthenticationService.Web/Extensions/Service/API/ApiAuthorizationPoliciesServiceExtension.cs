using AuthenticationService.API.AuthorizationPolicyRequirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.API
{
    public static class AuthorizationPoliciesServiceExtension
    {
        public static IServiceCollection AddApiAuthorizationPolicies(
            this IServiceCollection services
        )
        {
            services.AddAuthorization(options => {
                options.AddPolicy("ChangeUserAccess", policyBuilder => 
                    policyBuilder.Requirements.Add(new ChangeUserRequirement())
                );
            });

            services.AddScoped<IAuthorizationHandler, ChangeUserRequirementHandler>();
            
            return services;
        }
    }
}