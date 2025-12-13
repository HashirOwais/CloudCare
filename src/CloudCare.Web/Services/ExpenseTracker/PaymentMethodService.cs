using System.Net.Http.Json;
using CloudCare.Shared.Models;

namespace CloudCare.Web.Services.ExpenseTracker;

public class PaymentMethodService
{
    private readonly HttpClient _http;

    public PaymentMethodService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<List<PaymentMethod>> GetPaymentMethodsAsync()
    {
        return await _http.GetFromJsonAsync<List<PaymentMethod>>("api/paymentmethods") 
               ?? new List<PaymentMethod>();
    }
    
}