using AuthenticationService.API.Models.Responses.Token;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Responses.Token
{
    public class RefreshedTokenSetResponseDtoValidator : AbstractValidator<RefreshedTokenSetResponseDto>
    {
        public RefreshedTokenSetResponseDtoValidator()
        {
            RuleFor(tokenSetDto => tokenSetDto.AccessToken)
            .NotEmpty().WithMessage("Refreshed token set should contain an access token.");

            RuleFor(tokenSetDto => tokenSetDto.RefreshToken)
            .NotEmpty().WithMessage("Refreshed token set should contain a refresh token.");
        }
    }
}