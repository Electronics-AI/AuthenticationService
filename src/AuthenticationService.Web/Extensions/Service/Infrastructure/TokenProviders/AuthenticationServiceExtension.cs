using System;
using System.Text;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Infrastructure.Configurations.TokenProvider;
using AuthenticationService.Infrastructure.TokenProviders.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Web.Extensions.Service.Infrastructure.TokenProviders
{
    public static class AuthenticationServiceExtension
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services, 
            IConfiguration configuration 
        )
        { 
            JwtConfiguration jwtConfiguration = new JwtConfiguration();
            configuration.Bind("Jwt", jwtConfiguration);

            // !!! Use AssymetricSecurityKey for not authentication microservices 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => {
                        options.TokenValidationParameters = new TokenValidationParameters() {
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.AccessTokenSecret)),
                            ValidAudience = jwtConfiguration.Audience,
                            ValidIssuer = jwtConfiguration.Issuer,
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ClockSkew = TimeSpan.FromMinutes(jwtConfiguration.TokenExpirationClockSkewMinutes),
                        };
                    });

            return services;
        }

        public static IServiceCollection AddJwtTokenImplementation(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {   
            JwtConfiguration jwtConfiguration = new JwtConfiguration();
            configuration.Bind("Jwt", jwtConfiguration);

            services.AddSingleton(jwtConfiguration);

            services.AddTransient<IRefreshTokenValidator, JwtRefreshTokenValidator>();
            services.AddTransient<ITokenParser, JwtTokenParser>();
            services.AddTransient<ITokenSetGenerator, JwtTokenSetGenerator>();

            return services;
        }


    }
}