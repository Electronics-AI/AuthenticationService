using System.Collections.Generic;
using System.Security.Claims;
using AuthenticationService.Core.Domain.User;

namespace AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders
{
    public interface ITokenSetGenerator
    {
        TokenSet GenerateTokenSet(IEnumerable<Claim> claims);
    }
}