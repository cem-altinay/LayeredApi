using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace Sample.Security.Auth
{
    public class TokenBuilder : ITokenBuilder
    {
        public string Build(string name, string[] roles, DateTime expireDate)
        {
            var handler = new JwtSecurityTokenHandler();

            var claims = (roles.Select(userRole => new Claim(ClaimTypes.Role, userRole))).ToList();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = TokenAuthOption.Issuer,
                Audience = TokenAuthOption.Audience,
                SigningCredentials = TokenAuthOption.SigningCredentials,
                Subject = new ClaimsIdentity(new GenericIdentity(name, "Bearer"),
            claims
            ),
                Expires = expireDate
            });

            return handler.WriteToken(securityToken);

        }
    }
}
