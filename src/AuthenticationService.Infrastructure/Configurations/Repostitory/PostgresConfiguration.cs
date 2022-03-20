namespace AuthenticationService.Infrastructure.Configurations.Repository
{
    public class PostgresConfiguration
    {
        public string ConnectionString { get; set; }
        public string UsersTableName { get; set; }
    }
}