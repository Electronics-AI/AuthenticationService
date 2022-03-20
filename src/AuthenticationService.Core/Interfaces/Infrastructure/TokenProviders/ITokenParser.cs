using System;
using System.Collections.Generic;

namespace AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders
{
    public interface ITokenParser
    {
        Guid ExtractUserId(string token);
        Dictionary<string, int> ComposeTokenToExirationTimeMapping(params string[] token);
    }
}