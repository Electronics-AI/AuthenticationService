using AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers;


namespace AuthenticationService.Infrastructure.PasswordHashers
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}