using System;
using System.Net.Mime;
using System.Threading.Tasks;
using AuthenticationService.API.Models.Requests.Token;
using AuthenticationService.API.Models.Responses.Token;
using AuthenticationService.Core.Interfaces.Infrastructure.Loggers;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AuthenticationService.Core.Interfaces.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers
{
    [ApiController]
    [Route("api/tokens")]
    public class TokensController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<TokensController> _logger;

        public TokensController(
            ITokenService tokenService,
            IMapper mapper,
            ILogger<TokensController> logger
        )
        {
            _tokenService = tokenService ?? 
                throw new ArgumentNullException(nameof(tokenService));
            
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("refresh-tokens")]
        [ProducesDefaultResponseType()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResult<RefreshedTokenSetResponseDto>))]
        [Consumes(MediaTypeNames.Application.Json)] [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<RefreshedTokenSetResponseDto>> RefreshTokensAsync(
            [FromQuery]RefreshTokensRequestDto refreshTokensRequestDto
            )
        {
            _logger.Log(LogLevelTypes.Information, 
                        "Trying to refresh a user's token set by the given refresh token: {userRefreshTokensRefreshToken}.",
                        args: refreshTokensRequestDto.RefreshToken);
            
            TokenSet refreshedTokenSet = await _tokenService.RefreshTokensAsync(
                refreshToken: refreshTokensRequestDto.RefreshToken
            );

            var refreshedTokenSetDto = _mapper.Map<RefreshedTokenSetResponseDto>(refreshedTokenSet);

            _logger.Log(LogLevelTypes.Information, 
                        "Token set for a user with the refresh token: {userRefreshTokensRefreshToken}, " +
                        "has successfully been refreshed.",
                        args: refreshTokensRequestDto.RefreshToken);

            return Ok(refreshedTokenSetDto);
        }
    }
}