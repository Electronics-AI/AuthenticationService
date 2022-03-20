using AuthenticationService.Core.Domain.User;
using AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.CommonValidationRules;
using FluentValidation;

namespace AuthenticationService.Infrastructure.Validators.FluentValidation.Domain.User
{
    public class UserClaimsFluentValidator : AbstractValidator<UserClaims>
    {
        public UserClaimsFluentValidator()
        {
            RuleFor(userClaims => userClaims.Id)
            .NotEmpty().WithMessage("Id should not be empty");

            RuleFor(userClaims => userClaims.Role)
            .RoleIsValid("Any user role should not be null in an UserClaims object.");
        }
    }


}