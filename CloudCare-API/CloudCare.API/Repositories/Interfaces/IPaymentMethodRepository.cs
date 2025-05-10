using CloudCare.API.Models;

namespace CloudCare.API.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync(); // No userId needed, global only
}