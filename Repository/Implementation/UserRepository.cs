using Domain.Models;
using Repository.Infrastructure;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Sqlite");
        }

        public async Task AddUserAsync(User user)
        {
            string query = "INSERT INTO USER(Username, Login, Email, Password, MasterPassword) " +
                "VALUES(@Username, @Login, @Email, @Password, @MasterPassword);";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Username = user.Username,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                MasterPassword = user.MasterPassword
            };
            await conn.QueryAsync(query, parameters);
        }

        public async Task DeleteUserAsync(long userId)
        {
            string query = "DELETE FROM User WHERE User.ID = @UserId";

            using SqliteConnection conn = GetConnection();
            object parameters = new { UserId = userId };
            await conn.QueryAsync(query, parameters);
        }

        public async Task<User> FindUserAsync(long userId)
        {
            string query = "SELECT * FROM User U WHERE U.UserID = @UserId " +
                "INNER JOIN ServicePassword SP ON U.UserID = SP.UserID " +
                "INNER JOIN Device D ON U.UserID = D.UserID";

            User foundUser = new User();
            Dictionary<long, ServicePassword> passwords = new Dictionary<long, ServicePassword>();
            Dictionary<long, Device> devices = new Dictionary<long, Device>();

            using SqliteConnection conn = GetConnection();
            object parameters = new { UserId = userId };
            await conn.QueryAsync<User, ServicePassword, Device, User>
                (query,
                (user, password, device) =>
                {
                    foundUser.ID = user.ID;
                    foundUser.Username = user.Username;
                    foundUser.Email = user.Email;

                    if (!passwords.ContainsKey(password.ID))
                    {
                        passwords.Add(password.ID, password);
                    }
                    if (!devices.ContainsKey(device.ID))
                    {
                        devices.Add(device.ID, device);
                    }
                    return user;
                },
                parameters, splitOn: "ID");

            foundUser.ServicePasswords = passwords.Select(val => val.Value).ToList();
            foundUser.Devices = devices.Select(val => val.Value).ToList();

            return foundUser;
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }
    }
}
