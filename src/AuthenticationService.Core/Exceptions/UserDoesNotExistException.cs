using System;

namespace AuthenticationService.Core.Exceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException()
        {
            
        }
        
        public UserDoesNotExistException(string message) : base(message)
        {
            
        }
    }
}