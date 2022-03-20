using AuthenticationService.Core.Domain.User;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;

namespace AuthenticationService.Infrastructure.Repositories.EFCore
{
    public class EFCoreUserRepository : GenericEntityContextRepository<UserEntity>, IUserRepository
    {
        public EFCoreUserRepository(AuthenticationServiceDbContext context) : base(context)
        {
            
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await _context.Set<UserEntity>().FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<UserEntity> GetByUserNameAsync(string userName)
        {
            return await _context.Set<UserEntity>().FirstOrDefaultAsync(user => user.UserName == userName);
        }
    }
}