using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.Configurations.Repository;
using MongoDB.Driver;

namespace AuthenticationService.Infrastructure.Repositories.Mongo
{
    public class MongoUserRepository : GenericEntityMongoRepository<UserEntity>, IUserRepository
    {
        public MongoUserRepository(
            MongoConfiguration mongoDBConfiguration,
            IMongoDatabase mongoDatabase,
            IClientSessionHandle mongoSession

        ) : base(mongoDatabase, mongoSession, mongoDBConfiguration.UserCollectionName)
        {
    
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await (await _collection.FindAsync(_session, user => user.Email == email)).FirstOrDefaultAsync();
        }

        public async Task<UserEntity> GetByUserNameAsync(string userName)
        {
            return await (await _collection.FindAsync(_session, user => user.UserName == userName)).FirstOrDefaultAsync();
        }
    }
}