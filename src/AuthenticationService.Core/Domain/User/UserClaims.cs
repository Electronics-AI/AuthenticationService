using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthenticationService.Core.Domain.User
{
    public class UserClaims
    {
        public Guid Id { get; set; }
        public RoleTypes Role { get; set; }
        
        public UserClaims()
        {
            
        }
        
        public UserClaims(UserEntity user)
        {
            this.Id = user.Id;
            this.Role = user.Role;
        } 

        public UserClaims(IEnumerable<Claim> claims)
        {   
            foreach (Claim claim in claims) {

                switch (claim.Type) {
                    case ClaimTypes.NameIdentifier:
                        this.Id = Guid.Parse(claim.Value);
                        break;

                    case ClaimTypes.Role:
                        this.Role = (RoleTypes)Convert.ToInt32(claim.Value);
                        break;
                }
            }

        }

        public IEnumerable<Claim> ConvertToEnumerable()
        {   
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.NameIdentifier, this.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, this.Role.ToString("D")));

            return claims;
        }
    }
}