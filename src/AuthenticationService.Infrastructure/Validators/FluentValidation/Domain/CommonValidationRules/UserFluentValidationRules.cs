using System;
using AuthenticationService.Core.Domain.User;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.CommonValidationRules
{
    public static class UserFluentValidationRules
    {
        public static IRuleBuilderOptions<T, string> UserNameIsValid<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string notEmptyOptionMessage)
        {
            return ruleBuilder
                   .NotEmpty().WithMessage(notEmptyOptionMessage)
                   .Length(4, 25).WithMessage("Username should be 4 to 25 characters long.")
                   .Matches(@"^[\w\d_-]{4,25}$").WithMessage("Only '_' and '-' are allowed special characters in a username.");
        }

        public static IRuleBuilderOptions<T, string> PasswordIsValid<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string notEmptyOptionMessage)
        {
            return ruleBuilder
                   .NotEmpty().WithMessage(notEmptyOptionMessage)
                   .Length(8, 20).WithMessage("Password should be 8 to 20 characters long.")
                   .Matches(@"^[\w\d\@_-]{8,20}$").WithMessage("Only '@', '_' and '-' are allowed special characters in a password");
        }

        public static IRuleBuilderOptions<T, string> EmailIsValid<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            string notEmptyOptionMessage)
        {
            return ruleBuilder
                   .NotEmpty().WithMessage(notEmptyOptionMessage)
                   .EmailAddress();
        }

        public static IRuleBuilderOptions<T, GenderTypes> GenderIsValid<T>(
            this IRuleBuilder<T, GenderTypes> ruleBuilder,
            string notNullOptionMessage
            )
        {
            return ruleBuilder
                   .NotNull().WithMessage(notNullOptionMessage)
                   .IsInEnum().WithMessage($"Gender value should be in range from 0 to {Enum.GetNames(typeof(GenderTypes)).Length - 1}.");
        }

        public static IRuleBuilderOptions<T, DateTime> DateOfBirthIsValid<T>(
            this IRuleBuilder<T, DateTime> ruleBuilder,
            string notNullOptionMessage
            )
        {
            return ruleBuilder
                   .NotNull().WithMessage(notNullOptionMessage)
                   .LessThan(DateTime.Now).WithMessage($"Date of birth should be earlier than {DateTime.Now}.");
        }

        public static IRuleBuilderOptions<T, RoleTypes> RoleIsValid<T>(
            this IRuleBuilder<T, RoleTypes> ruleBuilder,
            string notNullOptionMessage
            )
        {
            return ruleBuilder
                   .NotNull().WithMessage(notNullOptionMessage)
                   .IsInEnum().WithMessage($"Role value should be in range from 0 to {Enum.GetNames(typeof(RoleTypes)).Length - 1}.");
        }
    }
}