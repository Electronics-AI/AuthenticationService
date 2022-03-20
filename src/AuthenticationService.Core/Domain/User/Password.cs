using AuthenticationService.Core.Interfaces.Infrastructure.PasswordHashers;

namespace AuthenticationService.Core.Domain.User
{
    public class Password
    {
        public PasswordTypes Type { get; set; }
        public string Value { get; set; }

        public void Hash(IPasswordHasher passwordHasher)
        {
            if (this.Type != PasswordTypes.Hashed) {
                this.Value = passwordHasher.HashPassword(Value);
                this.Type = PasswordTypes.Hashed;
            }
        }
    }
}