using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Infrastructure.Repositories.EFCore
{
    public abstract class GenericEntityContextRepository<TEntity> : IGenericEntityRepository<TEntity> where TEntity : Entity
    {
        protected readonly AuthenticationServiceDbContext _context;

        public GenericEntityContextRepository(AuthenticationServiceDbContext context)
        {
            _context = context ?? 
                throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(TEntity entity)
        {   
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public Task RemoveAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);

            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            _context.Set<TEntity>().RemoveRange(entities);

            return Task.CompletedTask;
        }

        public Task UpdateAsync(TEntity newEntity)
        {
            _context.Set<TEntity>().Update(newEntity);

            return Task.CompletedTask;
        }
    }
}