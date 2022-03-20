using AuthenticationService.API.Models.Requests.Token;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.Token
{
    public class RefreshTokensRequestDtoValidator : AbstractValidator<RefreshTokensRequestDto>
    {
        public RefreshTokensRequestDtoValidator()
        {
            RuleFor(refreshTokensDto => refreshTokensDto.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required to refresh tokens.");
        }
    }
}