using CloudCare.API.Models;

namespace CloudCare.API.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync(int userId); // Include global (UserId == null)
}