using System.Threading.Tasks;
using AuthenticationService.API.Controllers;
using AuthenticationService.API.Models.Requests.Token;
using AuthenticationService.API.Models.Responses.Token;
using AuthenticationService.Core.Interfaces.Infrastructure.Loggers;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Core.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AuthenticationService.UnitTests.API.Controllers
{
    public class TokensControllerTests
    {
        private readonly TokensController _tokensControllerUT;
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<TokensController>> _loggerMock = new();

        public TokensControllerTests()
        {
            _tokensControllerUT = new TokensController(
                _tokenServiceMock.Object, _mapperMock.Object, _loggerMock.Object
            );
        }

        [Fact]
        public async Task RefreshTokensAsync_WhenValidRefreshTokensDtoIsProvided_ShouldReturnRefreshedTokenSet()
        {
            RefreshTokensRequestDto refreshTokensRequestDto = new RefreshTokensRequestDto() {
                RefreshToken = "sample.valid.refreshtoken"
            };

            TokenSet refreshedTokenSet = new TokenSet() {
                AccessToken = "refreshed.access.token",
                RefreshToken = "refreshed.refresh.token"
            };

            RefreshedTokenSetResponseDto refreshedTokenSetResponseDto = new RefreshedTokenSetResponseDto() {
                AccessToken = refreshedTokenSet.AccessToken,
                RefreshToken = refreshedTokenSet.RefreshToken
            };

            _tokenServiceMock.Setup(tokenService => 
                tokenService.RefreshTokensAsync(refreshTokensRequestDto.RefreshToken))
                .ReturnsAsync(refreshedTokenSet)
                .Verifiable();
            
            _mapperMock.Setup(mapper =>
                mapper.Map<RefreshedTokenSetResponseDto>(refreshedTokenSet))
                .Returns(refreshedTokenSetResponseDto)
                .Verifiable();
            
            var refreshTokenResult = await _tokensControllerUT.RefreshTokensAsync(refreshTokensRequestDto);

            _tokenServiceMock.VerifyAll();
            _mapperMock.VerifyAll();

            var actionResult = Assert.IsType<ActionResult<RefreshedTokenSetResponseDto>>(refreshTokenResult);

            var okActionResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var refreshedTokenSetResult = Assert.IsType<RefreshedTokenSetResponseDto>(okActionResult.Value);

            Assert.Same(refreshedTokenSetResponseDto, refreshedTokenSetResult);
        }      

    }
}