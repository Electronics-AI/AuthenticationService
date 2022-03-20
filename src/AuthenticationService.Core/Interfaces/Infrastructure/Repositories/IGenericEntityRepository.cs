using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain;

namespace AuthenticationService.Core.Interfaces.Infrastructure.Repositories
{
    public interface IGenericEntityRepository<TEntity> where TEntity : Entity
    {
            Task<TEntity> GetByIdAsync(Guid id);
            Task<IEnumerable<TEntity>> GetAllAsync();
            Task AddAsync(TEntity entity);
            Task AddRangeAsync(IEnumerable<TEntity> entities);
            Task UpdateAsync(TEntity updatedEntity);
            Task RemoveAsync(TEntity entity);
            Task RemoveRangeAsync(IEnumerable<TEntity> entities);

    }
}