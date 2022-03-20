using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;

namespace AuthenticationService.Core.Interfaces.Infrastructure.Repositories
{
    public interface IUserRepository : IGenericEntityRepository<UserEntity>
    {
        Task<UserEntity> GetByEmailAsync(string email);
        Task<UserEntity> GetByUserNameAsync(string userName);
        
    }
}