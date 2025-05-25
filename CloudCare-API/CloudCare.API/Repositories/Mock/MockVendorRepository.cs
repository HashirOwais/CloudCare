using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockVendorRepository : IVendorRepository
{
    private readonly IEnumerable<Vendor> _vendors = new[]
    {
        new Vendor { Id = 1, Name = "Walmart" },
        new Vendor { Id = 2, Name = "Amazon" },
        new Vendor { Id = 3, Name = "Costco" },
        new Vendor { Id = 4, Name = "Staples" },
        new Vendor { Id = 5, Name = "Home Depot" },
        new Vendor { Id = 6, Name = "Best Buy" },
        new Vendor { Id = 7, Name = "Private Marketplace" }, // e.g. Facebook/Kijiji
        new Vendor { Id = 8, Name = "Local Vendor" },
        new Vendor { Id = 9, Name = "Government" },
        new Vendor { Id = 99, Name = "Miscellaneous" }
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