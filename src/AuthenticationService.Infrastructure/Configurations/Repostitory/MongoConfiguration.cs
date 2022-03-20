namespace AuthenticationService.Infrastructure.Configurations.Repository
{
    public class MongoConfiguration
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
    }
}