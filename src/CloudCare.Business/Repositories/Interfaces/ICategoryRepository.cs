using CloudCare.Data.Models;

namespace CloudCare.Business.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int categoryId);

    Task<Category?> GetByNameAsync(string categoryName); // Accepts name, returns nullable
}