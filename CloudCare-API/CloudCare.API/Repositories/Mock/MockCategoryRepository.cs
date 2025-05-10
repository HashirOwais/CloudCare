using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Food", UserId = null },
        new Category { Id = 2, Name = "Supplies", UserId = null },
        new Category { Id = 3, Name = "Transportation", UserId = 1 }, // user-specific
    };

    public Task<IEnumerable<Category>> GetAllAsync(int userId)
    {
        var result = _categories
            .Where(c => c.UserId == null || c.UserId == userId);

        return Task.FromResult(result);
    }
}