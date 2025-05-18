using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Food" },
        new Category { Id = 2, Name = "Supplies" },
        new Category { Id = 3, Name = "Transportation" }
    };

    public Task<IEnumerable<Category>> GetAllAsync()
    {
        return Task.FromResult(_categories.AsEnumerable());
    }

    public Task<Category?> GetByIdAsync(int categoryId)
    {
        var category = _categories.FirstOrDefault(c => c.Id == categoryId);
        return Task.FromResult(category);
    }
}