using System.Net.Http.Json;
using CloudCare.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CloudCare.Web.Services.ExpenseTracker;

public class PaymentMethodService
{
    private readonly HttpClient _http;
    private readonly ILogger<PaymentMethodService> _logger;

    public PaymentMethodService(HttpClient http, ILogger<PaymentMethodService> logger)
    {
        _http = http;
        _logger = logger;
    }
    
    public async Task<List<PaymentMethod>> GetPaymentMethodsAsync()
    {
        _logger.LogInformation("Fetching payment methods from API.");
        try
        {
            var paymentMethods = await _http.GetFromJsonAsync<List<PaymentMethod>>("api/paymentmethods");
            _logger.LogInformation("Successfully fetched {Count} payment methods.", paymentMethods?.Count ?? 0);
            return paymentMethods ?? new List<PaymentMethod>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching payment methods from API.");
            return new List<PaymentMethod>();
        }
    }
    
}