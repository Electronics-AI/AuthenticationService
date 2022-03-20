using System.Collections.Generic;

namespace AuthenticationService.API.Models.Responses.Error
{
    public class ErrorResponseDto
    {
        public IEnumerable<string> ErrorMessages { get; set; }
        
        public ErrorResponseDto(string errorMessage)
        {
            ErrorMessages = new List<string>() { errorMessage };
        }

        public ErrorResponseDto(IEnumerable<string> errorMessages)
        {
            ErrorMessages = errorMessages;
        }
    }
}