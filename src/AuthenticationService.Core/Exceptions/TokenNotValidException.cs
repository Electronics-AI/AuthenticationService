using System;

namespace AuthenticationService.Core.Exceptions
{
    public class TokenNotValidException : Exception
    {
        public TokenNotValidException()
        {
            
        }
        
        public TokenNotValidException(string message) : base(message)
        {
            
        }
    }
}