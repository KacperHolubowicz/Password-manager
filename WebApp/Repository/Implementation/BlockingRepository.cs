using Dapper;
using Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Repository.Infrastructure;
using System;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class BlockingRepository : IBlockingRepository
    {
        private readonly string connectionString;

        public BlockingRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Sqlite");
        }

        public async Task<bool> CheckIfCurrentlyBlocked(string ipAddress)
        {
            string query = "SELECT COUNT(1) From Blocking WHERE " +
                "IpAddress = @Ip AND BlockedUntil > @Now";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Ip = ipAddress,
                Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            return await conn.QuerySingleAsync<bool>(query, parameters);
        }

        public async Task<int> GetBlockCountAsync(string ipAddress)
        {
            string query = "SELECT COUNT(*) FROM Blocking WHERE " +
                "IpAddress = @Ip";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Ip = ipAddress
            };
            return await conn.QuerySingleAsync<int>(query, parameters);
        }

        public async Task SaveBlocking(Blocking blocking)
        {
            string query = "INSERT INTO Blocking " +
                "(IpAddress, Timestamp, BlockedUntil) " +
                "VALUES (@Ip, @Timestamp, @BlockedUntil)";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Ip = blocking.IpAddress,
                Timestamp = blocking.Timestamp,
                BlockedUntil = blocking.BlockedUntil
            };
            await conn.QueryAsync(query, parameters);
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
