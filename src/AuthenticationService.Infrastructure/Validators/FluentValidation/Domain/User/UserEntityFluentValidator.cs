using System;
using AuthenticationService.Core.Domain.User;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.CommonValidationRules;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.User
{
    public class UserEntityFluentValidator : AbstractValidator<UserEntity>
    {
        public UserEntityFluentValidator()
        {
            RuleFor(user => user.UserName)
            .UserNameIsValid("User entity should contain a username.");

            RuleFor(user => user.Email)
            .EmailIsValid("User entity should contain an email.");

            RuleFor(user => user.Gender)
            .GenderIsValid("User entity should contain a gender.");

            RuleFor(user => user.Role)
            .RoleIsValid("User entity's role cannot be null.");

            RuleFor(user => user.DateOfBirth)
            .DateOfBirthIsValid("User entity should contain a date of birth.");

            RuleFor(user => user.Password.Value)
            .PasswordIsValid("User entity should contain a password value.");

            RuleFor(user => user.LastLoginDate)
            .LessThan(DateTime.Now).WithMessage($"User entity's last login date should be earlier than {DateTime.Now}");

            RuleFor(user => user.CreationDate)
            .NotNull().WithMessage("User entity should contain a creation date.")
            .LessThan(DateTime.Now).WithMessage($"User entity's creation dare should be earlier than {DateTime.Now}");
        }
    }
}