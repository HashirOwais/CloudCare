using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Food & Snacks" },
        new Category { Id = 2, Name = "Educational Supplies" },
        new Category { Id = 3, Name = "Toys & Games" },
        new Category { Id = 4, Name = "Cleaning Supplies" },
        new Category { Id = 5, Name = "Utilities" },
        new Category { Id = 6, Name = "Office Supplies" },
        new Category { Id = 7, Name = "Furniture & Fixtures" },
        new Category { Id = 8, Name = "Repairs & Maintenance" },
        new Category { Id = 9, Name = "Transportation" },
        new Category { Id = 10, Name = "Insurance" },
        new Category { Id = 11, Name = "Professional Services" },
        new Category { Id = 12, Name = "Marketing & Advertising" },
        new Category { Id = 13, Name = "Staff Wages" },
        new Category { Id = 14, Name = "Training & Development" },
        new Category { Id = 15, Name = "Licenses & Permits" },
        new Category { Id = 99, Name = "Miscellaneous" }
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