using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockVendorRepository : IVendorRepository
{
    private readonly IEnumerable<Vendor> _vendors = new[]
    {
        new Vendor { Id = 1, Name = "Walmart" },
        new Vendor { Id = 2, Name = "Amazon" },
        new Vendor { Id = 3, Name = "Joe's Print Shop" },
        new Vendor { Id = 4, Name = "Local Grocery" }
    };

    public Task<IEnumerable<Vendor>> GetAllAsync()
    {
        return Task.FromResult(_vendors);
    }

    public Task<Vendor?> GetByIdAsync(int id)
    {
        return Task.FromResult(_vendors.FirstOrDefault(c => c.Id == id));
    }
}