using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.API.Repositories.EFCore;

public class VendorRepository : IVendorRepository
{
    public readonly CloudCareContext _cloudCareContext;

    public VendorRepository(CloudCareContext cloudCareContext)
    {
        _cloudCareContext = cloudCareContext ?? throw new ArgumentNullException(nameof(CloudCareContext));
    }
    public async Task<IEnumerable<Vendor>> GetAllAsync()
    {
        return await _cloudCareContext.Vendors.ToListAsync();
    }

    public Task<Vendor?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Vendor?> GetByVendorNameAsync(string vendorName)
    {
        return await _cloudCareContext.Vendors
            .FirstOrDefaultAsync(v => v.Name.ToLower() == vendorName.ToLower());
    }
}
