using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockPaymentMethodRepository : IPaymentMethodRepository
{
    private readonly IEnumerable<PaymentMethod> _paymentMethods = new[]
    {
        new PaymentMethod { Id = 1, Name = "Credit Card" },
        new PaymentMethod { Id = 2, Name = "Debit Card" },
        new PaymentMethod { Id = 3, Name = "Cash" },
        new PaymentMethod { Id = 4, Name = "E-Transfer" },
        new PaymentMethod { Id = 5, Name = "Cheque" }
    };

    public Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        return Task.FromResult(_paymentMethods);
    }

    public Task<PaymentMethod?> GetByIdAsync(int id)
    {
        var method = _paymentMethods.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(method);
    }
}