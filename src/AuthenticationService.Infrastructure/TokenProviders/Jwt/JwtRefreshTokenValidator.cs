using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Infrastructure.Configurations.TokenProvider;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Infrastructure.TokenProviders.Jwt
{
    public class JwtRefreshTokenValidator : IRefreshTokenValidator
    {
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtRefreshTokenValidator(
            JwtConfiguration jwtConfiguration
        )
        {
            _tokenValidationParameters = new TokenValidationParameters() {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.RefreshTokenSecret)),
                ValidIssuer = jwtConfiguration.Issuer,
                ValidAudience = jwtConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.FromMinutes(jwtConfiguration.TokenExpirationClockSkewMinutes)
            };

            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public bool Validate(string refreshToken)
        {
            try {
                _jwtSecurityTokenHandler.ValidateToken(
                    refreshToken,
                    _tokenValidationParameters,
                    out SecurityToken validatedToken
                    );
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
    }
}