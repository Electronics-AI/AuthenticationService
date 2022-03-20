using System.Collections.Generic;

namespace AuthenticationService.Core.Interfaces.Infrastructure.Validators
{
    public class ValidationResult
    {
        public bool IsValid { get; }
        public IEnumerable<ValidationFailure> Errors { get; }

        public ValidationResult(
            bool isValid,
            IEnumerable<ValidationFailure> errors
        )
        {
            IsValid = isValid;
            Errors = errors;   
        }
    }
}