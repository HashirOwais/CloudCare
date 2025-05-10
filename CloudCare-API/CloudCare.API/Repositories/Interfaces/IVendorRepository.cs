using CloudCare.API.Models;

namespace CloudCare.API.Repositories.Interfaces;

public interface IVendorRepository
{
    Task<IEnumerable<Vendor>> GetAllAsync(int userId);

}