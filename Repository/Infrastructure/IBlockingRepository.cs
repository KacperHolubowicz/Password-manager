using Domain.Models;
using System.Threading.Tasks;

namespace Repository.Infrastructure
{
    /// <summary>
    /// Repository layer interface for saving all blocks for particular Ip addresses
    /// </summary>
    public interface IBlockingRepository
    {
        public Task<int> GetBlockCountAsync(string ipAddress);
        public Task<bool> CheckIfCurrentlyBlocked(string ipAddress);
        public Task SaveBlocking(Blocking blocking);
    }
}
