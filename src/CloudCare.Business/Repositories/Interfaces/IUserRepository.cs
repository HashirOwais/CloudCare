using System;
using CloudCare.Data.Models;

namespace CloudCare.Business.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(string auth0Id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(string auth0Id);
    Task<bool> IsUserExistsAsync(string auth0Id);
}