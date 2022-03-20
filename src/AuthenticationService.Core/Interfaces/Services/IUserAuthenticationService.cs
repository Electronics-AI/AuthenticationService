using System.Threading.Tasks;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;

namespace AuthenticationService.Core.Interfaces.Services
{
    public interface IUserAuthenticationService 
    {
        Task<TokenSet> LoginByEmailAsync(string email, string password);
        Task<TokenSet> LoginByUserNameAsync(string userName, string password);
        Task LogoutAsync(TokenSet tokens);

    }
}