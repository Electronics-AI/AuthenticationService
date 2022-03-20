using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Infrastructure.Repositories.EFCore;

namespace AuthenticationService.Infrastructure.UnitsOfWork
{
    public class EFCoreUnitOfWork : BaseUnitOfWork
    {   
        private AuthenticationServiceDbContext _dbContext;

        public EFCoreUnitOfWork(
            AuthenticationServiceDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public override IUserRepository Users => _userRepository ??= 
            new EFCoreUserRepository(_dbContext);

        public override async Task CompleteAsync()
        {
            try {
                await _dbContext.SaveChangesAsync();
            }
            catch {
                _dbContext.ChangeTracker.Clear();
                throw;
            }
            finally {
                await _dbContext.DisposeAsync();
                resetRepositories();
            }
        }

        protected override Task disposeAsync(bool disposing)
        {   
            if (disposing) {
                _dbContext?.Dispose();
                _dbContext = null;
            }

            return Task.CompletedTask;
        }
    }
}