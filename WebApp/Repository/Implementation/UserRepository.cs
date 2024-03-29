﻿using Domain.Models;
using Repository.Infrastructure;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System;
using Repository.Exceptions;

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
            string query = "INSERT INTO USER(Username, Login, Email, Password, MasterPassword, Salt) " +
                "VALUES(@Username, @Login, @Email, @Password, @MasterPassword, @Salt);";

            await ValidateData(user);

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                Username = user.Username,
                Login = user.Login,
                Email = user.Email,
                Password = user.Password,
                MasterPassword = user.MasterPassword,
                Salt = user.Salt
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
                "LEFT JOIN ServicePassword SP ON U.UserID = SP.UserID " +
                "LEFT JOIN Device D ON U.UserID = D.UserID";
            object parameters = new { UserId = userId };

            User user = await FindUser(query, parameters);
            return user;
        }

        public async Task<bool> VerifyMasterPassword(long userId, byte[] masterPassword)
        {
            string query = "SELECT COUNT(1) FROM USER " +
                "WHERE ID = @UserId AND MasterPassword = @MasterPassword";

            using SqliteConnection conn = GetConnection();
            object parameters = new
            {
                UserId = userId,
                MasterPassword = masterPassword
            };
            bool verified = await conn.QuerySingleAsync<bool>(query, parameters);
            return verified;
        }

        public async Task<User> VerifyUserWithCredentials(string login, byte[] userPassword)
        {
            string query = "SELECT * FROM USER U " +
                "LEFT JOIN ServicePassword SP ON U.ID = SP.UserID " +
                "LEFT JOIN Device D ON U.ID = D.UserID " +
                "WHERE U.Login = @Login AND U.Password = @UserPassword";
            object parameters = new
            {
                Login = login,
                UserPassword = userPassword
            };

            User user = await FindUser(query, parameters);
            return user;
        }

        public async Task<byte[]> GetSaltById(long userId)
        {
            string query = "SELECT Salt FROM USER " +
                "WHERE ID = @UserId";

            object parameters = new {UserId = userId};
            using SqliteConnection conn = GetConnection();

            byte[] salt = await conn.QuerySingleOrDefaultAsync<byte[]>(query, parameters);
            
            if(salt == null)
            {
                throw new InvalidCredentialsException("Cannot get salt of this user");
            }
            return salt;
        }

        public async Task<byte[]> GetSaltByLogin(string login)
        {
            string query = "SELECT Salt FROM USER " +
                "WHERE Login = @Login";

            object parameters = new { Login = login };
            using SqliteConnection conn = GetConnection();

            byte[] salt = await conn.QuerySingleOrDefaultAsync<byte[]>(query, parameters);

            if(salt == null)
            {
                throw new InvalidCredentialsException("Cannot get salt of this user");
            }
            return salt;
        }

        public async Task<bool> FindUserWithEmail(string email)
        {
            string query = "SELECT COUNT(1) FROM USER WHERE Email = @Email";

            object parameters = new { Email = email };
            using SqliteConnection conn = GetConnection();

            bool doesEmailExist = await conn.QuerySingleAsync<bool>(query, parameters);
            return doesEmailExist;
        }

        private SqliteConnection GetConnection()
        {
            return new SqliteConnection(connectionString);
        }

        private async Task<User> FindUser(string query, object parameters)
        {
            User foundUser = new User();
            Dictionary<long, ServicePassword> passwords = new Dictionary<long, ServicePassword>();
            Dictionary<long, Device> devices = new Dictionary<long, Device>();
                using SqliteConnection conn = GetConnection();
                await conn.QueryAsync<User, ServicePassword, Device, User>
                    (query,
                    (user, password, device) =>
                    {
                        foundUser.ID = user.ID;
                        foundUser.Username = user.Username;
                        foundUser.Email = user.Email;

                        if (password != null && !passwords.ContainsKey(password.ID))
                        {
                            passwords.Add(password.ID, password);
                        }
                        if (device != null && !devices.ContainsKey(device.ID))
                        {
                            devices.Add(device.ID, device);
                        }
                        return user;
                    },
                    parameters, splitOn: "ID");

            if(foundUser.ID == 0)
            {
                throw new InvalidCredentialsException("Cannot find user with these credentials");
            }

            foundUser.ServicePasswords = passwords.Select(val => val.Value).ToList();
            foundUser.Devices = devices.Select(val => val.Value).ToList();
            return foundUser;
        }

        private async Task ValidateData(User user)
        {
            if(await FindUserWithEmail(user.Email)) 
            {
                throw new NonUniqueException("There is already a user with that email", NonUniqueFlag.Email);
            }
            if(await IsLoginUnique(user.Login))
            {
                throw new NonUniqueException("There is already a user with that login", NonUniqueFlag.Login);
            }
            if(await IsUsernameUnique(user.Username))
            {
                throw new NonUniqueException("There is already a user with that username", NonUniqueFlag.Username);
            }
        }

        private async Task<bool> IsUsernameUnique(string username)
        {
            string query = "SELECT COUNT(1) FROM USER WHERE Username = @Username";

            object parameters = new { Username = username };
            SqliteConnection conn = GetConnection();
            bool isUnique = await conn.QuerySingleAsync<bool>(query, parameters);
            return isUnique;
        }
        private async Task<bool> IsLoginUnique(string login)
        {
            string query = "SELECT COUNT(1) FROM USER WHERE Login = @Login";

            object parameters = new { Login = login };
            SqliteConnection conn = GetConnection();
            bool isUnique = await conn.QuerySingleAsync<bool>(query, parameters);
            return isUnique;
        }
    }
}
