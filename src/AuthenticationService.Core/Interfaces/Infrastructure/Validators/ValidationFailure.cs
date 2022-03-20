namespace AuthenticationService.Core.Interfaces.Infrastructure.Validators
{
    public class ValidationFailure
    {   
        public ValidationFailure(
            string errorMessage
        )
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; }
    }
}