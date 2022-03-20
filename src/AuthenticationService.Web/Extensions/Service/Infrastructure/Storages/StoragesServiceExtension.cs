using AuthenticationService.Core.Interfaces.Infrastructure.Storages;
using AuthenticationService.Infrastructure.Configurations.Repository;
using AuthenticationService.Infrastructure.Storages.TokenBlacklist.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Infrastructure.Storages
{
    public static class StoragesServiceExtension
    {
        public static IServiceCollection AddRedisTokenBlacklistStorage(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            RedisTokenBlacklistConfiguration redisTokenBlacklistConfiguration = 
                new RedisTokenBlacklistConfiguration();
            configuration.Bind("RedisTokenBlacklist", redisTokenBlacklistConfiguration);

            services.AddSingleton(redisTokenBlacklistConfiguration);
            services.AddScoped<ITokenBlacklistStorage, RedisTokenBlacklistStorage>();

            return services;
        }
    }
}