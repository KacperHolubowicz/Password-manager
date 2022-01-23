using Repository.Infrastructure;
using Services.DTO.Blocking;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class BlockingService : IBlockingService
    {
        private readonly IBlockingRepository blockingRepository;

        public BlockingService(IBlockingRepository blockingRepository)
        {
            this.blockingRepository = blockingRepository;
        }

        public async Task<bool> CheckIfCurrentlyBlocked(string ipAddress)
        {
            bool isBlocked = await blockingRepository.CheckIfCurrentlyBlocked(ipAddress);
            return isBlocked;
        }

        public async Task<int> GetBlockCountAsync(string ipAddress)
        {
            int blockCount = await blockingRepository.GetBlockCountAsync(ipAddress);
            return blockCount;
        }

        public async Task SaveBlocking(BlockingPostDTO blocking)
        {
            await blockingRepository.SaveBlocking(
                BlockingStaticMapper.GetBlockingFromDTO(blocking)
                );
        }
    }
}
