using Services.DTO.User;
using System.Security.Claims;

namespace Services.Infrastructure
{
    //todo czesc operacji zrzucic do authservice
    public interface IAuthService
    {
        public ClaimsPrincipal CreateClaims(UserGetDTO user);
    }
}
