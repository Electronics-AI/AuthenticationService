using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.Configurations.Repository;
using AuthenticationService.Infrastructure.Repositories.Dapper.Postgres;
using Npgsql;

namespace AuthenticationService.Infrastructure.UnitsOfWork.Dapper
{
    public class DapperPostgresUnitOfWork : DapperBaseUnitOfWork
    {   
        private readonly PostgresConfiguration _postgresConfiguration;

        public DapperPostgresUnitOfWork(
            PostgresConfiguration postgresConfiguration
            ) : base(new NpgsqlConnection(postgresConfiguration.ConnectionString))
        {
            _postgresConfiguration = postgresConfiguration;
        }
    

        public override IUserRepository Users => _userRepository ??= 
                new DapperPostgresUserRepository(_postgresConfiguration, _dbTransaction);
    }
}