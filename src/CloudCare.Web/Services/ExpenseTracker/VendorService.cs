using System.Net.Http.Json;
using CloudCare.Web.Models.ExpenseTracker;

namespace CloudCare.Web.Services.ExpenseTracker;

public class VendorService
{
    private readonly HttpClient _http;

    public VendorService(HttpClient http)
    {
        _http = http;
    }
    public async Task<List<Vendor>> GetVendorsAsync()
    {
        return await _http.GetFromJsonAsync<List<Vendor>>("api/vendors") 
               ?? new List<Vendor>();
    }
}