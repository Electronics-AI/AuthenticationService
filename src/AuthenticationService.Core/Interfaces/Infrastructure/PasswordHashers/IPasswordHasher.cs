namespace AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers
{
    public interface IPasswordHasher
    {
        string HashPassword(string plainTextPassword);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }
}