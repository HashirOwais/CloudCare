using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.API.Repositories.EFCore;

public class VendorRepository : IVendorRepository
{
    public readonly FinanceContext _FinanceContext;
    
    public VendorRepository(FinanceContext financeContext)
    {
        _FinanceContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));

    }
    public async Task<IEnumerable<Vendor>> GetAllAsync()
    {
        return await _FinanceContext.Vendors.ToListAsync();
    }

    public Task<Vendor?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}