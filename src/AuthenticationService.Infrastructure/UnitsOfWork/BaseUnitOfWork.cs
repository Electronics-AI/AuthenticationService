using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;

namespace AuthenticationService.Infrastructure.UnitsOfWork
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        protected IUserRepository _userRepository;
    
        public abstract IUserRepository Users { get; }

        public abstract Task CompleteAsync();

        protected abstract Task disposeAsync(bool disposing);

        public void Dispose()
        {
            disposeAsync(true);
            GC.SuppressFinalize(this);
        }

        public ValueTask DisposeAsync()
        {
            disposeAsync(true);
            GC.SuppressFinalize(this);

            return ValueTask.CompletedTask;
        }

        protected void resetRepositories()
        {
            _userRepository = null;
        }

        ~BaseUnitOfWork() {
            disposeAsync(false);
        }
    }
}