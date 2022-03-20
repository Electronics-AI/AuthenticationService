using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Core.Interfaces.Infrastructure.Storages;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Core.Interfaces.Services;

namespace AuthenticationService.Core.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly ITokenBlacklistStorage _tokenBlacklist;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenValidator _refreshTokenValidator;
        private readonly ITokenParser _tokenParser;
        private readonly ITokenSetGenerator _tokenSetGenerator;

        public TokenService(
            ITokenBlacklistStorage tokenBlacklist,
            IUnitOfWork unitOfWork,
            IRefreshTokenValidator refreshTokenValidator,
            ITokenParser tokenParser,
            ITokenSetGenerator tokenSetGenerator
        )
        {
            _tokenBlacklist = tokenBlacklist ??
                throw new ArgumentNullException(nameof(tokenBlacklist));

            _unitOfWork = unitOfWork ?? 
                throw new ArgumentNullException(nameof(unitOfWork));
            
            _refreshTokenValidator = refreshTokenValidator ??
                throw new ArgumentNullException(nameof(refreshTokenValidator));

            _tokenParser = tokenParser ??
                throw new ArgumentNullException(nameof(tokenParser));
            
            _tokenSetGenerator = tokenSetGenerator ??
                throw new ArgumentNullException(nameof(tokenSetGenerator));
        }

        public async Task<TokenSet> RefreshTokensAsync(string refreshToken)
        {

            bool refreshTokenIsValid = _refreshTokenValidator.Validate(refreshToken);
            if (!refreshTokenIsValid) {
                throw new TokenNotValidException("Refresh token is not valid");
            }

            var tokenToExpirationTimeMapping = _tokenParser.ComposeTokenToExirationTimeMapping(refreshToken);
            await _tokenBlacklist.AddTokensAsync(tokenToExpirationTimeMapping);

            Guid userId = _tokenParser.ExtractUserId(refreshToken);

            UserEntity user = await _unitOfWork.Users.GetByIdAsync(userId);

            var claims = new UserClaims(user).ConvertToEnumerable();

            return _tokenSetGenerator.GenerateTokenSet(claims);
            
        }

        public async Task<bool> CheckTokenInBlacklistAsync(string token)
        {
            var tokenToExpirationTimeMapping = _tokenParser.ComposeTokenToExirationTimeMapping(token);

            return await _tokenBlacklist.CheckTokenInBlacklistAsync(tokenToExpirationTimeMapping);
        }
    }
}