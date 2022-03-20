using System;

namespace AuthenticationService.Core.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {   
        public UserAlreadyExistsException()
        {
            
        }
        
        public UserAlreadyExistsException(string message) : base(message)
        {
            
        }
    }
}