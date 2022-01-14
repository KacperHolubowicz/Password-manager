using Services.DTO.User;
using System.Security.Claims;

namespace Services.Infrastructure
{
    public interface IAuthService
    {
        public ClaimsPrincipal CreateClaims(UserGetDTO user);
    }
}
