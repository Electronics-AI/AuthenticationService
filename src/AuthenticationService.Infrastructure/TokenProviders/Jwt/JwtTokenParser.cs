using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;

namespace AuthenticationService.Infrastructure.TokenProviders.Jwt
{
    public class JwtTokenParser : ITokenParser
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtTokenParser()
        {
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public Dictionary<string, int> ComposeTokenToExirationTimeMapping(params string[] tokens)
        {
            var tokenToExpirationTimeMapping = new Dictionary<string, int>();

            foreach(string token in tokens) {
                string pureToken = extractToken(token);

                tokenToExpirationTimeMapping.Add(
                    key: pureToken,
                    value: extractTokenExpirationTime(pureToken)
                );
            }

            return tokenToExpirationTimeMapping;
        }

        public Guid ExtractUserId(string token)
        {
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(extractToken(token));
            
            return Guid.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        }

        private string extractToken(string token)
        {
            return token.Replace("Bearer ", string.Empty);
        }

        private int extractTokenExpirationTime(string token)
        {
            var jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(extractToken(token));

            return Int32.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "exp").Value);
        }
    }
}