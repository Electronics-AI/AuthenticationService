using AuthenticationService.API.Models.Requests.User;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.CommonValidationRules;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.User
{
    public class RegisterUserRequestDtoValidator : AbstractValidator<RegisterUserRequestDto>
    {
        public RegisterUserRequestDtoValidator()
        {
            RuleFor(registerUserDto => registerUserDto.UserName)
            .UserNameIsValid("Username is required to register a new user.");

            RuleFor(registerUserDto => registerUserDto.Email)
            .EmailIsValid("Email is required to register a new user.");

            RuleFor(registerUserDto => registerUserDto.Gender)
            .GenderIsValid("Gender is required to register a new user.");

            RuleFor(registerUserDto => registerUserDto.DateOfBirth)
            .DateOfBirthIsValid("Date of birth is required to register a new user.");
            
            RuleFor(registerUserDto => registerUserDto.Password)
            .PasswordIsValid("Password is required to register a new user.");

            RuleFor(registerUserDto => registerUserDto.ConfirmPassword)
            .Equal(registerUserDto => registerUserDto.Password).WithMessage("Confirm password and password should match.");
        }
    }
}