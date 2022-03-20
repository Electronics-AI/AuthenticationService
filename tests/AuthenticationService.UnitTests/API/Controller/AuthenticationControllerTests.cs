using System;
using System.Linq;
using System.Threading.Tasks;
using AuthenticationService.API.Controllers;
using AuthenticationService.API.Models.Requests.Authentication;
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
    public class AuthenticationControllerTests
    {   
        private readonly AuthenticationController _authenticationControllerUT;
        private readonly Mock<IUserAuthenticationService> _userAuthenticationServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<AuthenticationController>> _loggerMock = new();
        
        public AuthenticationControllerTests()
        {
            _authenticationControllerUT = new AuthenticationController(
                _userAuthenticationServiceMock.Object, _mapperMock.Object, _loggerMock.Object
            );
        }

        [Fact]
        public async Task LoginAsync_WhenLoginUserDtoContainsValidEmailAndPassword_ShouldReturnTokenSet()
        {
            LoginUserRequestDto loginUserRequestDto = sampleValidLoginUserRequestDto;
            loginUserRequestDto.UserName = String.Empty;

            TokenSet refreshedLoginTokenSet = sampleValidLoginTokenSet;

            RefreshedTokenSetResponseDto loginTokenSetDto = new RefreshedTokenSetResponseDto() {
                AccessToken = refreshedLoginTokenSet.AccessToken,
                RefreshToken = refreshedLoginTokenSet.RefreshToken
            };

            _userAuthenticationServiceMock.Setup(auth =>
                auth.LoginByEmailAsync(loginUserRequestDto.Email, loginUserRequestDto.Password))
                .ReturnsAsync(refreshedLoginTokenSet)
                .Verifiable();

            _mapperMock.Setup(mapper =>
                mapper.Map<RefreshedTokenSetResponseDto>(refreshedLoginTokenSet))
                .Returns(loginTokenSetDto)
                .Verifiable();
            
            var loginResult = await _authenticationControllerUT.LoginAsync(loginUserRequestDto);

            _userAuthenticationServiceMock.VerifyAll();
            _mapperMock.VerifyAll();

            assertLoginAsyncReturnsOkObjectResultWithValidRefreshedTokenSetResponseDto(
                loginTokenSetDto, loginResult
            );
        }

        [Fact] 
        public async Task LoginAsync_WhenLoginUserDtoContainsValidUserNameAndPassword_ShouldReturnTokenSet()
        {
            LoginUserRequestDto loginUserRequestDto = sampleValidLoginUserRequestDto;
            loginUserRequestDto.Email = String.Empty;

            TokenSet refreshedLoginTokenSet = sampleValidLoginTokenSet;

            RefreshedTokenSetResponseDto loginTokenSetDto = new RefreshedTokenSetResponseDto() {
                AccessToken = refreshedLoginTokenSet.AccessToken,
                RefreshToken = refreshedLoginTokenSet.RefreshToken
            };

            _userAuthenticationServiceMock.Setup(auth => 
                auth.LoginByUserNameAsync(loginUserRequestDto.UserName, loginUserRequestDto.Password))
                .ReturnsAsync(refreshedLoginTokenSet)
                .Verifiable();

            _mapperMock.Setup(mapper =>
                mapper.Map<RefreshedTokenSetResponseDto>(refreshedLoginTokenSet))
                .Returns(loginTokenSetDto)
                .Verifiable();
            
            var loginResult = await _authenticationControllerUT.LoginAsync(loginUserRequestDto);

            _userAuthenticationServiceMock.VerifyAll();
            _mapperMock.VerifyAll();

            assertLoginAsyncReturnsOkObjectResultWithValidRefreshedTokenSetResponseDto(
                loginTokenSetDto, loginResult
            );
        }

        [Fact]
        public async Task LogoutAsync_WhenLogoutUserDtoContainsValidRefreshToken_ShouldLogoutUser()
        {
            LogoutUserRequestDto logoutUserRequestDto = new LogoutUserRequestDto() {
                RefreshToken = "sample.logout.validrefreshtoken"
            };

            _userAuthenticationServiceMock.Setup(auth =>
                auth.LogoutAsync(It.IsAny<TokenSet>()))
                .Verifiable();

            var logoutResult = await _authenticationControllerUT.LogoutAsync(logoutUserRequestDto);

            _userAuthenticationServiceMock.VerifyAll();

            Assert.IsType<NoContentResult>(logoutResult);
        }

        private void assertLoginAsyncReturnsOkObjectResultWithValidRefreshedTokenSetResponseDto(
            RefreshedTokenSetResponseDto loginTokenSetDto, 
            ActionResult<RefreshedTokenSetResponseDto> loginResult
        )
        {
            var actionResult = Assert.IsType<ActionResult<RefreshedTokenSetResponseDto>>(loginResult);

            var okActionResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            var loginTokenSetResult = Assert.IsType<RefreshedTokenSetResponseDto>(okActionResult.Value);

            Assert.Same(loginTokenSetDto, loginTokenSetResult);
        }

        private LoginUserRequestDto sampleValidLoginUserRequestDto => new LoginUserRequestDto() {
                Password = "passw",
                UserName = "username",
                Email = "useremail@gmail.com"
            };

        private TokenSet sampleValidLoginTokenSet = new TokenSet() {
                AccessToken = "refreshed.access.token",
                RefreshToken = "refreshed.refresh.token"
            };
    }
}