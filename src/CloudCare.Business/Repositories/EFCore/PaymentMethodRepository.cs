using CloudCare.Data.DbContexts;
using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.Business.Repositories.EFCore;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    public readonly CloudCareContext _cloudCareContext;

    public PaymentMethodRepository(CloudCareContext cloudCareContext)
    {
        _cloudCareContext = cloudCareContext ?? throw new ArgumentNullException(nameof(CloudCareContext));
    }
    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        return await _cloudCareContext.PaymentMethods.ToListAsync();
    }

    public Task<PaymentMethod?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<PaymentMethod?> GetByNameAsync(string name)
    {
        return await _cloudCareContext.PaymentMethods
            .FirstOrDefaultAsync(pm => pm.Name.ToLower() == name.ToLower());
    }
}
