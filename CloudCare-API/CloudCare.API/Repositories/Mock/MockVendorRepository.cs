using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockVendorRepository : IVendorRepository
{
    private readonly IEnumerable<Vendor> _vendors = new[]
    {
        new Vendor { Id = 1, Name = "Walmart", UserId = null },
        new Vendor { Id = 2, Name = "Amazon", UserId = null },
        new Vendor { Id = 3, Name = "Joe's Print Shop", UserId = 2 },
        new Vendor { Id = 4, Name = "Local Grocery", UserId = 1 }
    };

    public Task<IEnumerable<Vendor>> GetAllAsync(int userId)
    {
        var result = _vendors.Where(v => v.UserId == null || v.UserId == userId);
        return Task.FromResult(result);
    }
}