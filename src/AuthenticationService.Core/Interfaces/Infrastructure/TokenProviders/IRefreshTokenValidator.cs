namespace AuthenticationService.Core.Interfaces.Infrastructure.TokenProviders
{
    public interface IRefreshTokenValidator
    {
        bool Validate(string refreshToken);
    }
}