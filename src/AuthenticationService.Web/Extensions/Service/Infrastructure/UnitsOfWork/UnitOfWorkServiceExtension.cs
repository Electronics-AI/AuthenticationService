using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.UnitsOfWork;
using AuthenticationService.Infrastructure.UnitsOfWork.Dapper;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationService.Web.Extensions.Service.Infrastructure.UnitsOfWork
{
    public static class UnitOfWorkServiceExtension
    {
        public static IServiceCollection AddDapperPostgresUnitOfWork(
            this IServiceCollection services
        )
        {
            services.AddScoped<IUnitOfWork, DapperPostgresUnitOfWork>();
            return services;
        }

        public static IServiceCollection AddMongoUnitOfWork(
            this IServiceCollection services
        )
        {
            services.AddScoped<IUnitOfWork, MongoUnitOfWork>();
            return services;
        }

        public static IServiceCollection AddEFCoreUnitOfWork(
            this IServiceCollection services
        )
        {
            services.AddScoped<IUnitOfWork, EFCoreUnitOfWork>();
            return services;
        }

    }
}