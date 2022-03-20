using AuthenticationService.API.Models.Requests.User;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.CommonValidationRules;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Model.Requests.User
{
    public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
    {
        public UpdateUserRequestDtoValidator()
        {
            RuleFor(updateUserDto => updateUserDto.UserName)
            .UserNameIsValid("User name is required to update a user.");
            
            RuleFor(updateUserDto => updateUserDto.Email)
            .EmailIsValid("Email is required to update a user.");

            RuleFor(updateUserDto => updateUserDto.Gender)
            .GenderIsValid("Gender is required to update a user.");

            RuleFor(updateUserDto => updateUserDto.DateOfBirth)
            .DateOfBirthIsValid("Date of birth is required to update a user.");

            RuleFor(updateUserDto => updateUserDto.Role)
            .RoleIsValid("Any user role should not be null when updating a user.");
        }
    }
}