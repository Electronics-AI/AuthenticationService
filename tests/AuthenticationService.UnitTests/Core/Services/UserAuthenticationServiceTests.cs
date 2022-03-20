using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Interfaces.Infrastructure.Loggers;
using AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Core.Interfaces.Infrastructure.Storages;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Core.Services.Authentication;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTests.Core.Services.Authentication
{
    public class UserAuthenticationServiceTests
    {
        private readonly UserAuthenticationService _userAuthenticationServiceUT;
        private readonly Mock<ITokenBlacklistStorage> _tokenBlacklistMock = new();
        private readonly Mock<ITokenSetGenerator> _tokenSetGeneratorMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<ITokenParser> _tokenParserMock = new();
        private readonly Mock<ILogger<UserAuthenticationService>> _loggerMock = new();

        public UserAuthenticationServiceTests()
        {
            _userAuthenticationServiceUT = new AuthenticationService.Core.Services.Authentication.UserAuthenticationService(
                _tokenBlacklistMock.Object,
                _tokenSetGeneratorMock.Object, _unitOfWorkMock.Object,
                _passwordHasherMock.Object, _tokenParserMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task LoginByEmailAsync_WhenUserWithLoginEmailDoesNotExist_ShouldThrowUserDoesNotExistException()
        {
            string nonExistingUserLoginEmail = "useremail@gmail.com"; 
            string userLoginPassword = "passw";

            _unitOfWorkMock.Setup(unitOfWork =>
                unitOfWork.Users.GetByEmailAsync(nonExistingUserLoginEmail))
                .ReturnsAsync(() => null)
                .Verifiable();

            Func<Task> act = () => _userAuthenticationServiceUT.LoginByEmailAsync(
                nonExistingUserLoginEmail, userLoginPassword
            );

            await Assert.ThrowsAsync<UserDoesNotExistException>(act);

            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task LoginByEmailAsync_WhenLoginPasswordNotValid_ShouldThrowPasswordNotValidException()
        {
            string userLoginEmail = "email@icloud.com";
            string userLoginPassword = "loginPassw";
            string userHashedPassword = "$ODN850$6WM";

            _passwordHasherMock.Setup(hasher =>
                hasher.VerifyPassword(It.IsAny<string>(), userLoginPassword))
                .Returns(false)
                .Verifiable();
            
            _unitOfWorkMock.Setup(unitOfWork => unitOfWork.Users.GetByEmailAsync(userLoginEmail))
                .ReturnsAsync(new UserEntity() {
                    Password = new Password() {
                        Value = userHashedPassword
                        }
                    })
                .Verifiable();

            Func<Task> act = () => _userAuthenticationServiceUT.LoginByEmailAsync(userLoginEmail, userLoginPassword);

            await Assert.ThrowsAsync<PasswordNotValidException>(act);

            _passwordHasherMock.VerifyAll();
            _unitOfWorkMock.VerifyAll();
        }

        [Fact]
        public async Task LogoutAsync_WhenProvidedTokenSetIsValid_ShouldAddTokensToTokenBlacklist()
        {
            string logoutAccessToken = "access.token.sample";
            int logoutAccessTokenExpTime = 754; 
            string logoutRefreshToken = "refresh.token.sample";
            int logoutRefreshTokenExpTime = 111;

            TokenSet logoutTokenSet = new TokenSet() {
                AccessToken = logoutAccessToken, 
                RefreshToken = logoutRefreshToken
            };

            var tokensToExpTimeMapping = new Dictionary<string, int>();
            tokensToExpTimeMapping.Add(logoutTokenSet.AccessToken, logoutAccessTokenExpTime);
            tokensToExpTimeMapping.Add(logoutTokenSet.RefreshToken, logoutRefreshTokenExpTime);

            _tokenParserMock.Setup(parser =>
                parser.ComposeTokenToExirationTimeMapping(logoutAccessToken, logoutRefreshToken))
                .Returns(tokensToExpTimeMapping);

            _tokenBlacklistMock.Setup(tokenBlacklist =>
                tokenBlacklist.AddTokensAsync(tokensToExpTimeMapping))
                .Verifiable();

            await _userAuthenticationServiceUT.LogoutAsync(logoutTokenSet);

            _unitOfWorkMock.VerifyAll();
        }
    }
}