using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthenticationService.Core.Interfaces.Infrastructure.Storages
{
    public interface ITokenBlacklistStorage : IDisposable
    {
        Task AddTokensAsync(Dictionary<string, int> tokenToExpirationTimeMapping);
        Task<bool> CheckTokenInBlacklistAsync(Dictionary<string, int> tokenToExpirationTimeMapping);
    }
}