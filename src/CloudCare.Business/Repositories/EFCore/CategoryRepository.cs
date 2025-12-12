using CloudCare.Data.DbContexts;
using CloudCare.Data.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.Business.Repositories.EFCore;

public class CategoryRepository : ICategoryRepository
{
    public readonly CloudCareContext _CloudCareContext;

    public CategoryRepository(CloudCareContext financeContext)
    {
        _CloudCareContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _CloudCareContext.Categories.ToListAsync();
    }

    public Task<Category?> GetByIdAsync(int categoryId)
    {
        throw new NotImplementedException();
    }

    public async Task<Category?> GetByNameAsync(string categoryName)
    {
        return await _CloudCareContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == categoryName.ToLower());
    }
}
