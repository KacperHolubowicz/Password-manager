namespace Services.DTO.Blocking
{
    public class BlockingStaticMapper
    {
        public static Domain.Models.Blocking GetBlockingFromDTO
            (BlockingPostDTO blockingPost)
        {
            return new Domain.Models.Blocking()
            {
                IpAddress = blockingPost.IpAddress,
                Timestamp = blockingPost.Timestamp,
                BlockedUntil = blockingPost.BlockedUntil
            };
        }
    }
}
