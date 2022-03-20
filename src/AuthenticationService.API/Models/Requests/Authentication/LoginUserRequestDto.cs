using AuthenticationService.Core.Domain.User;

namespace AuthenticationService.API.Models.Requests.Authentication
{
    public class LoginUserRequestDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        
        }
}