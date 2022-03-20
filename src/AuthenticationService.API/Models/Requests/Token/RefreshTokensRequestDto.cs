using System.ComponentModel.DataAnnotations;
using AuthenticationService.Core.Domain.User;

namespace AuthenticationService.API.Models.Requests.Token
{
    public class RefreshTokensRequestDto
    {
        public string RefreshToken { get; set; }
    }
}