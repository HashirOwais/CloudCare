using System.Net.Http.Json;
using CloudCare.Shared.Models;
using Microsoft.Extensions.Logging;

namespace CloudCare.Web.Services.ExpenseTracker;

public class CategoryService
{
    private readonly HttpClient _http;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(HttpClient http, ILogger<CategoryService> logger)
    {
        _http = http;
        _logger = logger;
    }
    
    public async Task<List<Category>> GetCategoriesAsync()
    {
        _logger.LogInformation("Fetching categories from API.");
        try
        {
            var categories = await _http.GetFromJsonAsync<List<Category>>("api/categories");
            _logger.LogInformation("Successfully fetched {Count} categories.", categories?.Count ?? 0);
            return categories ?? new List<Category>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories from API.");
            return new List<Category>();
        }
    }
    
}