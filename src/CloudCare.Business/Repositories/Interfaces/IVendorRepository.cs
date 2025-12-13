using CloudCare.Shared.Models;

namespace CloudCare.Business.Repositories.Interfaces;

public interface IVendorRepository
{
    Task<IEnumerable<Vendor>> GetAllAsync();
    Task<Vendor?> GetByIdAsync(int id);

    Task<Vendor?> GetByVendorNameAsync(string vendorName);

}