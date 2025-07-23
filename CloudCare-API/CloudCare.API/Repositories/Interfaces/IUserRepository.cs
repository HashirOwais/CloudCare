using System;
using CloudCare.API.Models;

namespace CloudCare.API.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<bool> IsUserExistsAsync(int id);
}
