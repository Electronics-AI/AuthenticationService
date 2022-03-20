using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using MongoDB.Driver;

namespace AuthenticationService.Infrastructure.Repositories.Mongo
{
    public abstract class GenericEntityMongoRepository<T> : IGenericEntityRepository<T> where T : Entity
    {
        protected readonly IMongoCollection<T> _collection;
        protected readonly IClientSessionHandle _session;

        public GenericEntityMongoRepository(
            IMongoDatabase mongoDatabase,
            IClientSessionHandle mongoSession,
            string collectionName
            )
        {            
            _collection = mongoDatabase.GetCollection<T>(collectionName);
            _session = mongoSession;
        }
        
        public async Task AddAsync(T entity)
        {
            
            await _collection.InsertOneAsync(_session, entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _collection.InsertManyAsync(_session, entities);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return (await _collection.FindAsync(_session, _ => true)).ToEnumerable().AsQueryable();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await (await _collection.FindAsync(_session, entity => entity.Id == id)).FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            await _collection.DeleteOneAsync(_session, ent => ent.Id == entity.Id);
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            Guid[] entityIds = new Guid[entities.Count()];
            
            for (int idx = 0; idx < entities.Count(); idx++) {
                entityIds[idx] = entities.ElementAt(idx).Id;
            }

            await _collection.DeleteManyAsync(_session, entity => Array.Exists(entityIds, entId => entId == entity.Id));
        }

        public async Task UpdateAsync(T newEntity)
        {
            await _collection.ReplaceOneAsync(_session, entity => entity.Id == newEntity.Id, newEntity);
        }
    }
}