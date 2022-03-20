
using System.ComponentModel.DataAnnotations;

namespace AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders
{
    public class TokenSet
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}