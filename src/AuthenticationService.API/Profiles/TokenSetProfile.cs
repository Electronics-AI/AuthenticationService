using AuthenticationService.API.Models.Responses.Token;
using AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders;
using AutoMapper;

namespace AuthenticationService.API.Profiles
{
    public class TokenSetProfile : Profile
    {
        public TokenSetProfile()
        {
            CreateMap<TokenSet, RefreshedTokenSetResponseDto>()
                .ForMember(tokenSetDto => tokenSetDto.AccessToken, opt =>
                           opt.MapFrom(tokenSet => tokenSet.AccessToken))
                .ForMember(tokenSetDto => tokenSetDto.RefreshToken, opt =>
                           opt.MapFrom(tokenSet => tokenSet.RefreshToken));
        }
    }
}