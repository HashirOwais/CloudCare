using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockPaymentMethodRepository : IPaymentMethodRepository
{
    private readonly IEnumerable<PaymentMethod> _paymentMethods = new[]
    {
        new PaymentMethod { Id = 1, Name = "Cash" },
        new PaymentMethod { Id = 2, Name = "Credit Card" },
        new PaymentMethod { Id = 3, Name = "E-Transfer" }
    };

    public Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
        return Task.FromResult(_paymentMethods);
    }
}