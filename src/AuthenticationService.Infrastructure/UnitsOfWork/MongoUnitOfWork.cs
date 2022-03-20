using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.Configurations.Repository;
using AuthenticationService.Infrastructure.Repositories.Mongo;
using MongoDB.Driver;

namespace AuthenticationService.Infrastructure.UnitsOfWork
{
    public class MongoUnitOfWork : BaseUnitOfWork
    {   
        private IMongoDatabase _mongoDatabase;
        private Lazy<Task<IClientSessionHandle>> _mongoSession;
        private readonly MongoClient _mongoClient;
        private readonly MongoConfiguration _mongoConfiguration;

        public MongoUnitOfWork(MongoConfiguration mongoConfiguration)
        {
            _mongoConfiguration = mongoConfiguration;

            _mongoClient = new MongoClient(_mongoConfiguration.ConnectionString);
            
            _mongoSession = new Lazy<Task<IClientSessionHandle>>(async () => {
                var mongoSession = await _mongoClient.StartSessionAsync();
                mongoSession.StartTransaction();
                return mongoSession;
            });

            _mongoDatabase = _mongoClient.GetDatabase(mongoConfiguration.DatabaseName);       
        }

        public override IUserRepository Users => _userRepository ??=
            new MongoUserRepository(_mongoConfiguration, _mongoDatabase, _mongoSession.Value.Result);
        

        public override async Task CompleteAsync()
        {
            try {
                await _mongoSession.Value.Result.CommitTransactionAsync();
            }
            catch {
                await _mongoSession.Value.Result.AbortTransactionAsync();
                throw;
            }
            finally {
                _mongoSession.Value.Result.StartTransaction();
                resetRepositories();
            }

        }

        protected override Task disposeAsync(bool disposing)
        {
            if (disposing) {
                _mongoSession.Value.Result?.Dispose();
                _mongoSession = null;
            }

            return Task.CompletedTask;
        }
    }
}