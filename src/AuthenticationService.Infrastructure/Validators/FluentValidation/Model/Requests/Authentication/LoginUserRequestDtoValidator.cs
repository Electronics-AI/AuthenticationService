using System;
using AuthenticationService.API.Models.Requests.Authentication;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.CommonValidationRules;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.Authentication
{
    public class LoginUserRequestDtoValidator : AbstractValidator<LoginUserRequestDto>
    {
        public LoginUserRequestDtoValidator()
        {
            RuleFor(loginUserDto => loginUserDto.Password)
            .PasswordIsValid("Password is required to log in");

            When(loginUserDto => String.IsNullOrEmpty(loginUserDto.Email), () =>
            {
                RuleFor(loginUserDto => loginUserDto.UserName)
                .UserNameIsValid("Username or email is required to log in.");
            });

            When(loginUserDto => String.IsNullOrEmpty(loginUserDto.UserName), () => 
            {
                RuleFor(loginUserDto => loginUserDto.Email)
                .EmailIsValid("Username or email is required to log in.");
            });
        }
    }
}