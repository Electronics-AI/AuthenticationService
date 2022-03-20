using System;
using System.ComponentModel;


namespace AuthenticationService.Core.Domain.User
{
    public class UserEntity : Entity
    {   
        public string UserName { get; set; }

        public string Email { get; set; }   

        public GenderTypes Gender { get; set; }

        [DefaultValue(RoleTypes.User)]
        public RoleTypes Role { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? CreationDate { get; set; }

        public Password Password { get; set; }

    }
}
