using System;
using System.Collections.Generic;
using FluentValidation;


namespace AuthenticationService.Infrastructure.Validators.FluentValidation
{
    public class GenericFluentValidator<T> : Core.Interfaces.Infrastructure.Validators.IValidator<T> where T : class
    {
        private readonly AbstractValidator<T> _validator;

        public GenericFluentValidator(
            AbstractValidator<T> validator
        )
        {
            _validator = validator ??
                throw new ArgumentNullException(nameof(validator));
        }

        public Core.Interfaces.Infrastructure.Validators.ValidationResult Validate(T model)
        {
            global::FluentValidation.Results.ValidationResult fluentValidationResult = _validator.Validate(model);

            Core.Interfaces.Infrastructure.Validators.ValidationResult coreValidationResult =
                convertFluentValidationResultToCore(fluentValidationResult);

            return coreValidationResult;
        }

        private Core.Interfaces.Infrastructure.Validators.ValidationResult convertFluentValidationResultToCore(
            global::FluentValidation.Results.ValidationResult fluentValidationResult
            )
        {
            List<Core.Interfaces.Infrastructure.Validators.ValidationFailure> coreErrors = 
                new List<Core.Interfaces.Infrastructure.Validators.ValidationFailure>();

            foreach (var fleuntValidationFailure in fluentValidationResult.Errors) {
                coreErrors.Add(new Core.Interfaces.Infrastructure.Validators.ValidationFailure(
                    errorMessage: fleuntValidationFailure.ErrorMessage
                    ));
            }

            Core.Interfaces.Infrastructure.Validators.ValidationResult coreValidationResult =
                new Core.Interfaces.Infrastructure.Validators.ValidationResult(
                    isValid: fluentValidationResult.IsValid,
                    errors: coreErrors
                );

            return coreValidationResult;
            
        }
    }
}