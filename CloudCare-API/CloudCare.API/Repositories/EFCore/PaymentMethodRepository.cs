using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.API.Repositories.EFCore;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    public readonly FinanceContext _FinanceContext;

    public PaymentMethodRepository(FinanceContext financeContext)
    {
        _FinanceContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));

    }
    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        return await _FinanceContext.PaymentMethods.ToListAsync();
    }

    public Task<PaymentMethod?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public  async Task<PaymentMethod?> GetByNameAsync(string name)
    {
        return await _FinanceContext.PaymentMethods
            .FirstOrDefaultAsync(pm => pm.Name.ToLower() == name.ToLower());
    }
}