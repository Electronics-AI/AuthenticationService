using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;

namespace AuthenticationService.Core.Interfaces.Services
{
    public interface ITokenService
    {
        Task<TokenSet> RefreshTokensAsync(string refreshToken);
        Task<bool> CheckTokenInBlacklistAsync(string token);
    }
}