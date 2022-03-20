using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Infrastructure.Configurations.TokenProvider;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.Infrastructure.TokenProviders.Jwt
{
    public class JwtTokenSetGenerator : ITokenSetGenerator
    {
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtTokenSetGenerator(JwtConfiguration jwtConfiguration)
        {
            _jwtConfiguration = jwtConfiguration;
        }
        
        public TokenSet GenerateTokenSet(IEnumerable<Claim> claims)
        {
            TokenSet tokenSet = new TokenSet() {
                AccessToken = generateAccessToken(claims),
                RefreshToken = generateRefreshToken(claims)
            };

            return tokenSet;
        }

        protected string generateRefreshToken(IEnumerable<Claim> claims)
        {
            return generateToken(
                secretKey: _jwtConfiguration.RefreshTokenSecret,
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                expirationMinutes: _jwtConfiguration.RefreshTokenExpirationMinutes,
                claims: claims
                );
        }

        protected string generateAccessToken(IEnumerable<Claim> claims)
        {
            
            return generateToken(
                secretKey: _jwtConfiguration.AccessTokenSecret,
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                expirationMinutes: _jwtConfiguration.AccessTokenExpirationMinutes,
                claims: claims
                );
        }

        private string generateToken(string secretKey, string issuer, string audience,
                                     int expirationMinutes, IEnumerable<Claim> claims = null)
        {
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials signingCredentials = new SigningCredentials(
                key: securityKey,
                algorithm: SecurityAlgorithms.HmacSha256
            );

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience, 
                claims: claims, 
                notBefore: DateTime.UtcNow, 
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes), 
                signingCredentials: signingCredentials
            ); 
            
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}