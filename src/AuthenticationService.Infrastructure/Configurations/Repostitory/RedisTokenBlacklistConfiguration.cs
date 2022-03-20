namespace AuthenticationService.Infrastructure.Configurations.Repository
{
    public class RedisTokenBlacklistConfiguration 
    {
        public string ConnectionString { get; set; }
        public string TokenBlacklistSortedSetName { get; set; }
    }
}