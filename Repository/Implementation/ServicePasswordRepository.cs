using Dapper;
using Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Repository.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class ServicePasswordRepository : IServicePasswordRepository
    {
        private readonly string connectionString;

        public ServicePasswordRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Sqlite");
        }

        public async Task CreatePasswordAsync(long ownerId, ServicePassword password)
        {
            string query = "INSERT INTO ServicePassword " +
                "(UserID, Description, Password, IV) VALUES " +
                "(@UserId, @Description, @Password, @IV)";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                UserId = ownerId,
                Description = password.Description,
                Password = password.Password,
                IV = password.IV
            };
            await conn.QueryAsync(query, parameters);
        }

        public async Task DeletePasswordAsync(long passwordId)
        {
            string query = "DELETE FROM ServicePassword SP WHERE SP.ID = @PassId";

            using SqliteConnection conn = GetConnection();
            object parameters = new { PassId = passwordId };
            await conn.QueryAsync(query, parameters);
        }

        public async Task DeleteUserWithPasswordsAsync(long userId)
        {
            string query = "DELETE FROM ServicePassword SP WHERE SP.UserID = @UserId";

            using SqliteConnection conn = GetConnection();
            object parameters = new { UserId = userId };
            await conn.QueryAsync(query, parameters);
        }

        public async Task<List<ServicePassword>> FindAllPasswordsAsync(long userId)
        {
            string query = "SELECT * FROM ServicePassword SP WHERE SP.UserID = @UserId";

            using SqliteConnection conn = GetConnection();
            object parameters = new { UserId = userId };
            IEnumerable<ServicePassword> passwords = await conn.QueryAsync<ServicePassword>(query, parameters);
            return passwords.ToList();
        }

        public async Task<ServicePassword> FindPasswordById(long passwordId)
        {
            string query = "SELECT * FROM ServicePassword SP WHERE SP.ID = @PassId";

            using SqliteConnection conn = GetConnection();
            object parameters = new { PassId = passwordId };
            ServicePassword password = await conn.QueryFirstOrDefaultAsync<ServicePassword>(query, parameters);
            return password;
        }

        public async Task UpdatePasswordAsync(ServicePassword password, long passwordId)
        {
            string query = "UPDATE ServicePassword SET Description = @Description, " +
                "Password = @Password, IV = @IV WHERE ID = @PasswordId";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Description = password.Description,
                Password = password.Password,
                PasswordId = passwordId,
                IV = password.IV
            };
            await conn.QueryAsync(query, parameters);
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
