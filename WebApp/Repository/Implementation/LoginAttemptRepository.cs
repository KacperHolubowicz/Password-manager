using Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Repository.Infrastructure;
using System.Threading.Tasks;
using Dapper;
using System;

namespace Repository.Implementation
{
    public class LoginAttemptRepository : ILoginAttemptRepository
    {
        private readonly string connectionString;
        private readonly int attemptCountReset;

        public LoginAttemptRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Sqlite");
            attemptCountReset = int.Parse(configuration["Safety:AttemptResetMinutes"]);
        }

        public async Task<int> GetFrequentAttemptCountAsync(string ipAddress)
        {
            string query = "SELECT COUNT(*) FROM LoginAttempt WHERE " +
                "Timestamp > @CountTime AND IpAddress = @Ip AND Successful = 0";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                CountTime = DateTime.Now.AddMinutes(-attemptCountReset).ToString("yyyy-MM-dd HH:mm:ss"),
                Ip = ipAddress
            };
            int attemptCount = await conn.QuerySingleAsync<int>(query, parameters);
            return attemptCount;
        }

        public async Task SaveAttemptsAsync(LoginAttempt attempt)
        {
            string query = "INSERT INTO LoginAttempt " +
                "(IpAddress, Timestamp, Successful)" +
                "VALUES (@Ip, @Timestamp, @Successful)";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Ip = attempt.IpAddress,
                Timestamp = attempt.Timestamp,
                Successful = attempt.Successful ? 1 : 0
            };
            await conn.QueryAsync(query, parameters);
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
