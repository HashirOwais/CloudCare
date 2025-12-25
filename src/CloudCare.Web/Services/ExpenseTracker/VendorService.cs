using System.Net.Http.Json;
using CloudCare.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CloudCare.Web.Services.ExpenseTracker;

public class VendorService
{
    private readonly HttpClient _http;
    private readonly ILogger<VendorService> _logger;

    public VendorService(HttpClient http, ILogger<VendorService> logger)
    {
        _http = http;
        _logger = logger;
    }
    public async Task<List<Vendor>> GetVendorsAsync()
    {
        _logger.LogInformation("Fetching vendors from API.");
        try
        {
            var vendors = await _http.GetFromJsonAsync<List<Vendor>>("api/vendors");
            _logger.LogInformation("Successfully fetched {Count} vendors.", vendors?.Count ?? 0);
            return vendors ?? new List<Vendor>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching vendors from API.");
            return new List<Vendor>();
        }
    }
}