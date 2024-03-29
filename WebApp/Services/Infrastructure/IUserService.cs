﻿using Services.DTO.User;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    /// <summary>
    /// Service layer class for all backend logic connected with users
    /// </summary>
    public interface IUserService
    {
        public Task<UserGetDTO> FindUserAsync(long userId);
        public Task AddUserAsync(UserPostDTO user);
        public Task DeleteUserAsync(long userId);
        public Task<UserGetDTO> LoginUser(string login, string passwordPlainText);
        public Task<bool> FindUserWithEmail(string email);
    }
}
