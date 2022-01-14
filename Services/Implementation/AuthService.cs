using Microsoft.AspNetCore.Authentication.Cookies;
using Services.DTO.User;
using Services.Infrastructure;
using System.Collections.Generic;
using System.Security.Claims;

namespace Services.Implementation
{
    public class AuthService : IAuthService
    {
        public ClaimsPrincipal CreateClaims(UserGetDTO user)
        {
            Claim[] claims = new Claim[3]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim("ID", user.ID.ToString())
            };

            return new ClaimsPrincipal(
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)
                );
        }
    }
}
