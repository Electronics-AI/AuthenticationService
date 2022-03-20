using System;

namespace AuthenticationService.Core.Exceptions
{
    public class PasswordNotValidException : Exception
    {   
        public PasswordNotValidException()
        {
            
        }
        
        public PasswordNotValidException(string message) : base(message)
        {
            
        }
    }
}