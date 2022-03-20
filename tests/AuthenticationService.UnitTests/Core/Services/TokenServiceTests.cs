using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthenticationService.Core.Exceptions;
using AuthenticationService.Core.Interfaces.Infrastructure.Repositories;
using AuthenticationService.Core.Interfaces.Infrastructure.Storages;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Core.Services.Token;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTests.Core.Services.Token
{
    public class TokenServiceTests
    {   
        private readonly TokenService _tokenServiceUT; 
        private readonly Mock<ITokenBlacklistStorage> _tokenBlacklistMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IRefreshTokenValidator> _refreshTokenValidatorMock = new();
        private readonly Mock<ITokenParser> _tokenParserMock = new();
        private readonly Mock<ITokenSetGenerator> _tokenSetGeneratorMock = new();

        public TokenServiceTests()
        {
            _tokenServiceUT = new TokenService(
                _tokenBlacklistMock.Object,
                _unitOfWorkMock.Object, _refreshTokenValidatorMock.Object,
                _tokenParserMock.Object, _tokenSetGeneratorMock.Object
            );
        }

        [Fact]
        public async Task RefreshTokensAsync_WhenRefreshTokenIsNotValid_ShouldThrowTokenIsNotValidException()
        {
            string notValidRefreshToken = "notvalidtoken";

            _refreshTokenValidatorMock.Setup(validator =>
                validator.Validate(notValidRefreshToken))
                .Returns(false)
                .Verifiable();
            
            Func<Task> act = () => _tokenServiceUT.RefreshTokensAsync(notValidRefreshToken);
            
            await Assert.ThrowsAsync<TokenNotValidException>(act);

            _refreshTokenValidatorMock.VerifyAll();
        }

        [Fact]
        public async Task CheckTokenInBlacklistAsync_WhenTokenFromBlacklistIsPassed_ShouldReturnTrue()
        {      
            string tokenFromBlacklist = "token.from.blacklist";

            _tokenBlacklistMock.Setup(tokenBlacklist =>
                tokenBlacklist.CheckTokenInBlacklistAsync(It.IsAny<Dictionary<string, int>>()))
                .ReturnsAsync(true)
                .Verifiable();

            bool tokenInBlacklist = await _tokenServiceUT.CheckTokenInBlacklistAsync(tokenFromBlacklist);

            _tokenBlacklistMock.VerifyAll();
            
            Assert.True(tokenInBlacklist);
        }

    }
}