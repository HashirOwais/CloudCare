using CloudCare.Shared.Models;

namespace CloudCare.Business.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
    Task<IEnumerable<PaymentMethod>> GetAllAsync(); // Global only
    Task<PaymentMethod?> GetByIdAsync(int id);       // Accepts ID, returns nullable

    Task<PaymentMethod?> GetByNameAsync(string name); // Accepts name, returns nullable
}