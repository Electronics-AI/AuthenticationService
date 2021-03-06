using System;
using AuthenticationService.Core.Domain.User;

namespace AuthenticationService.API.Models.Requests.User
{
    public class RegisterUserRequestDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }   

        public GenderTypes Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Password { get; set; }
        
        public string ConfirmPassword { get; set; }

    }
}