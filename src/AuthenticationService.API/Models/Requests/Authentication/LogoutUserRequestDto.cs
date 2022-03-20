namespace AuthenticationService.API.Models.Requests.Authentication
{
    public class LogoutUserRequestDto
    {
        public string RefreshToken { get; set; }
    }
}