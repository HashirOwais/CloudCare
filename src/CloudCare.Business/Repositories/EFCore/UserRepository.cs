using CloudCare.Data.DbContexts;
using CloudCare.Data.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.Business.Repositories.EFCore;

public class UserRepository : IUserRepository
{
    public readonly CloudCareContext _cloudcareContext;

    public UserRepository(
        CloudCareContext cloudcareContext)
    {
        _cloudcareContext = cloudcareContext;
    }

    public async Task<User?> GetUserByIdAsync(string auth0Id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<User> AddUserAsync(User user)
    {
        _cloudcareContext.Users.Add(user); //the user will have the ID from DB after this
        await _cloudcareContext.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteUserAsync(string auth0Id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserExistsAsync(string auth0Id)
    {
        var user = await _cloudcareContext.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Auth0Id == auth0Id);
        return user != null;
    }
}
