using Services.DTO.Blocking;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    /// <summary>
    /// Service layer class for all backend logic connected with blocks given to
    /// ip addresses after 5 unsuccessful login attempts.
    /// Block duration should be increasing with its count for one particular ip address.
    /// </summary>
    public interface IBlockingService
    {
        public Task<int> GetBlockCountAsync(string ipAddress);
        public Task<bool> CheckIfCurrentlyBlocked(string ipAddress);
        public Task SaveBlocking(BlockingPostDTO blocking);
    }
}
