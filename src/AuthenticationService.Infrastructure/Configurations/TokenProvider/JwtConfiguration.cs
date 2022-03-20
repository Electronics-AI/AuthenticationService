namespace AuthenticationService.Infrastructure.Configurations.TokenProvider
{
    public class JwtConfiguration 
    {
        public string AccessTokenSecret { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public string RefreshTokenSecret { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public int TokenExpirationClockSkewMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}