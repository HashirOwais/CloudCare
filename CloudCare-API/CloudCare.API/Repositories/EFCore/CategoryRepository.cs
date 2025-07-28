using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.API.Repositories.EFCore;

public class CategoryRepository : ICategoryRepository
{
    public readonly FinanceContext _FinanceContext;

    public CategoryRepository(FinanceContext financeContext)
    { 
        _FinanceContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _FinanceContext.Categories.ToListAsync();
        

    }

    public Task<Category?> GetByIdAsync(int categoryId)
    {
        throw new NotImplementedException();
    }
}