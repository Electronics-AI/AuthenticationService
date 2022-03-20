using System;
using System.Threading.Tasks;

namespace AuthenticationService.Core.Interfaces.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IUserRepository Users { get; }
        Task CompleteAsync();
    }
}