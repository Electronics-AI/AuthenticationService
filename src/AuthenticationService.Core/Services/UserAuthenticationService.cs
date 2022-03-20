using System;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers;
using AuthenticationService.Core.Interfaces.Infrastructure.Loggers;
using AuthenticationService.Core.Interfaces.Services;
using AuthenticationService.Core.Interfaces.Infrastructure.Storages;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;

namespace AuthenticationService.Core.Services.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {   
        private readonly ITokenBlacklistStorage _tokenBlacklist;
        private readonly ITokenSetGenerator _tokenSetGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenParser _tokenParser;
        private readonly ILogger<UserAuthenticationService> _logger;
        
        public UserAuthenticationService(
            ITokenBlacklistStorage tokenBlacklist,
            ITokenSetGenerator tokenSetGenerator,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            ITokenParser tokenParser,
            ILogger<UserAuthenticationService> logger
        )
        {
            _tokenBlacklist = tokenBlacklist ??
                throw new ArgumentNullException(nameof(tokenBlacklist));

            _tokenSetGenerator = tokenSetGenerator ??
                throw new ArgumentNullException(nameof(tokenSetGenerator));
            
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            
            _passwordHasher = passwordHasher ??
                throw new ArgumentNullException(nameof(passwordHasher));

            _tokenParser = tokenParser ??
                throw new ArgumentNullException(nameof(tokenParser));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TokenSet> LoginByEmailAsync(string email, string password)
        {   
            _logger.Log(LogLevelTypes.Information,
                        "Trying to get a user from the Users repository by the given email: {userLoginEmail}.",
                        args: email);
            
            UserEntity loginUser = (await _unitOfWork.Users.GetByEmailAsync(email));

            if (loginUser == null) {
                _logger.Log(LogLevelTypes.Information,
                            "User with the email: {userLoginEmail} does not exist.",
                            args: email);
                throw new UserDoesNotExistException($"User with the email '{email}' not found.");
            } 

            TokenSet tokenSet = await loginByUserAsync(loginUser, password);
            
            _logger.Log(LogLevelTypes.Information,
                        "User with the email: {userLoginEmail}; has successfully been logged in",
                        args: email);

            return tokenSet;
        }

        public async Task<TokenSet> LoginByUserNameAsync(string userName, string password)
        {
            _logger.Log(LogLevelTypes.Information,
                        "Trying to get a user from the Users repository by the given username: {userLoginUserName}.",
                        args: userName);

            UserEntity loginUser = (await _unitOfWork.Users.GetByUserNameAsync(userName));

            if (loginUser == null) {
                _logger.Log(LogLevelTypes.Information,
                            "User with the username: {userLoginUserName} does not exist.",
                            args: userName);
                throw new UserDoesNotExistException($"User with the username '{userName}' not found.");
            } 

            await loginByUserAsync(loginUser, password);

            TokenSet tokenSet = generateTokenSet(loginUser);
            _logger.Log(LogLevelTypes.Information,
                        "User with the username: {userLoginUserName}; has successfully been logged in",
                        args: userName);

            return tokenSet;
        }

        public async Task LogoutAsync(TokenSet tokenSet)
        {               
            _logger.Log(LogLevelTypes.Information,
                        "Trying to add the given token set: {@userLogoutTokenSet}; to the token blacklist.",
                        args: tokenSet);

            var tokenToExpTimeMapping = 
                _tokenParser.ComposeTokenToExirationTimeMapping(tokenSet.AccessToken, tokenSet.RefreshToken);

            await _tokenBlacklist.AddTokensAsync(tokenToExpTimeMapping);
            
            _logger.Log(LogLevelTypes.Information,
                        "Token set: {userLogoutTokenSet}; has successfully been added to the token blacklist.",
                        args: tokenSet);

        }

        private async Task<TokenSet> loginByUserAsync(UserEntity user, string password)
        {   
            bool userPasswordIsValid = _passwordHasher.VerifyPassword(user.Password.Value, password);
            if (!userPasswordIsValid) {
                _logger.Log(LogLevelTypes.Information,
                            "Wrong password provided for a user with the email: {userLoginEmail}; " + 
                            "and the username: {userLoginUserName}.",
                            args: new string[]{user.Email, user.UserName});

                throw new PasswordNotValidException("Password is not valid.");
            }
            
            user.LastLoginDate = DateTime.Now;

            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
            
            return generateTokenSet(user);
        }

        private TokenSet generateTokenSet(UserEntity user)
        {

            var claims = new UserClaims(user).ConvertToEnumerable();

            TokenSet tokenSet =  _tokenSetGenerator.GenerateTokenSet(claims);

            _logger.Log(LogLevelTypes.Information,
                        "Token set for a user with the email: {userLoginEmail}; and the username: {userLoginUserName}; " + 
                        "has successfully been generated.",
                        args: new string[]{user.Email, user.UserName});

            return tokenSet;
        }

    }
}
