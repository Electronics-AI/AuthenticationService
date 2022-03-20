using AuthenticationService.Core.Domain;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Interfaces.Infrastructure.Storages;
using AuthenticationService.Infrastructure.Configurations.Repository;
using AuthenticationService.Infrastructure.Repositories.EFCore;
using AuthenticationService.Infrastructure.Storages.TokenBlacklist.Redis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using Npgsql;

namespace AuthenticationService.Web.Extensions.Service.Infrastructure.Repositories
{
    public static class RepositoriesServiceExtension
    {
        public static IServiceCollection AddEFCorePostgresConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            PostgresConfiguration postgresConfiguration = 
                new PostgresConfiguration();
            configuration.Bind("Postgres", postgresConfiguration);

            services.AddDbContext<AuthenticationServiceDbContext>(options =>
                options.UseNpgsql(postgresConfiguration.ConnectionString));

            return services;
        }

        public static IServiceCollection AddMongoConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            MongoConfiguration mongoConfiguration = 
                new MongoConfiguration();
            configuration.Bind("Mongo", mongoConfiguration);

            services.AddSingleton(mongoConfiguration);
            
            BsonClassMap.RegisterClassMap<Entity>(userMap => {
                    userMap.AutoMap();
                    userMap.MapIdMember(user => user.Id)
                           .SetIdGenerator(CombGuidGenerator.Instance);
                    userMap.SetIsRootClass(true);
                });
            BsonClassMap.RegisterClassMap<UserEntity>();

            return services;
        }

        public static IServiceCollection AddDapperPostgresConfiguration(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            PostgresConfiguration postgresConfiguration = 
                new PostgresConfiguration();
            configuration.Bind("Postgres", postgresConfiguration);

            services.AddSingleton(postgresConfiguration);

            return services;
        }

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