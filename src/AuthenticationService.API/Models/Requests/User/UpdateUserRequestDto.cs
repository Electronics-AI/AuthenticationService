using System;
using AuthenticationService.Core.Domain.User;

namespace AuthenticationService.API.Models.Requests.User
{
    public class UpdateUserRequestDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }   

        public RoleTypes Role { get; set; }

        public GenderTypes Gender { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        
    }
}