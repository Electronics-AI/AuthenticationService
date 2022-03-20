
namespace AuthenticationService.API.Models.Responses.Token
{
    public class RefreshedTokenSetResponseDto
    {
        public string AccessToken { get; set; }
        
        public string RefreshToken { get; set; }
    }
}