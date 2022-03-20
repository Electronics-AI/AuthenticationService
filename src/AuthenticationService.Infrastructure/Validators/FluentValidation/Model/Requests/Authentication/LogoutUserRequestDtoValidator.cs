using AuthenticationService.API.Models.Requests.Authentication;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.Authentication
{
    public class LogoutUserRequestDtoValidator : AbstractValidator<LogoutUserRequestDto>
    {
        public LogoutUserRequestDtoValidator()
        {
            RuleFor(logoutUserDto => logoutUserDto.RefreshToken)
            .NotEmpty().WithMessage("Refresh token is required to log out.");
        }
    }
}