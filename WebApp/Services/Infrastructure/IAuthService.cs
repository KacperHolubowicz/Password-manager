using Services.DTO.User;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface IAuthService
    {
        public ClaimsPrincipal CreateClaims(UserGetDTO user);
        public Task<string> CheckUserBlock(string remoteIp);
        public Task<UserGetDTO> TryLoginUser(string login, string password, string remoteIp);
    }
}
