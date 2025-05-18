using CloudCare.API.Models;

namespace CloudCare.API.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync(); // Global only
    Task<PaymentMethod?> GetByIdAsync(int id);       // Accepts ID, returns nullable
}