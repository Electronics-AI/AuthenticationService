using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AuthenticationService.API.Models.Requests.Authentication;
using AuthenticationService.API.Models.Responses.Token;
using AuthenticationService.Core.Interfaces.Infrastructure.Loggers;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Core.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace AuthenticationService.API.Controllers
{   
    // Controllers annotated with the [ApiController] attribute automatically validate model state and return a 400 response.
    // For more information, see Automatic HTTP 400 responses. Web API controllers don't have to check ModelState.IsValid if 
    // they have the [ApiController] attribute. In that case, an automatic HTTP 400 response containing error details is returned
    // when model state is invalid.
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _userAuthenticationService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IUserAuthenticationService userAuthenticationService,
            IMapper mapper,
            ILogger<AuthenticationController> logger
            )
        {
            _userAuthenticationService = userAuthenticationService ?? 
                throw new ArgumentNullException(nameof(userAuthenticationService));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _logger = logger ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpPost("login")]
        [ProducesDefaultResponseType()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<RefreshedTokenSetResponseDto>))]
        [Consumes(MediaTypeNames.Application.Json)] [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<RefreshedTokenSetResponseDto>> LoginAsync(
            [FromBody]LoginUserRequestDto loginUserRequestDto
            )
        {               
            TokenSet tokenSet;
            
            if (!string.IsNullOrEmpty(loginUserRequestDto.Email)) {
                _logger.Log(LogLevelTypes.Information,
                            "Trying to log in a user by the given email: {userLoginEmail}.", 
                            args: loginUserRequestDto.Email);
    
                tokenSet = await _userAuthenticationService.LoginByEmailAsync(
                        email: loginUserRequestDto.Email,
                        password: loginUserRequestDto.Password
                        );
                
                _logger.Log(LogLevelTypes.Information,
                            "User with the email: {userLoginEmail}; has successfully been logged in.",
                            args: loginUserRequestDto.Email);
            }
            else {
                _logger.Log(LogLevelTypes.Information,
                            "Trying to log in a user by the given username: {userLoginUserName}.",
                            args: loginUserRequestDto.UserName);

                tokenSet = await _userAuthenticationService.LoginByUserNameAsync(
                        userName: loginUserRequestDto.UserName,
                        password: loginUserRequestDto.Password
                        );

                _logger.Log(LogLevelTypes.Information,
                            "User with the username: {userLoginUserName}; has successfully been logged in.",
                            args: loginUserRequestDto.UserName);              
            }

            var refreshedTokenSetDto = _mapper.Map<RefreshedTokenSetResponseDto>(tokenSet);
            
            return Ok(refreshedTokenSetDto);
        }
        
        [Authorize]
        [HttpPost("logout")]
        [ProducesDefaultResponseType()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> LogoutAsync(
            [FromBody]LogoutUserRequestDto logoutUserRequestDto
        )
        {   
            TokenSet tokenSet = new TokenSet() {
                AccessToken = Request?.Headers[HeaderNames.Authorization].ToString(),
                RefreshToken = logoutUserRequestDto.RefreshToken
                };

            _logger.Log(LogLevelTypes.Information, 
                        "Trying to log out a user by the given token set: {@userLogoutTokenSet}.",
                        args: tokenSet);

            await _userAuthenticationService.LogoutAsync(tokenSet);

            _logger.Log(LogLevelTypes.Information, 
                        "User with the token set: {@userLogoutTokenSet}, has successfully been logged out.",
                        args: tokenSet);

            return NoContent();
        }
    }
}