using System.Net.Http.Json;
using CloudCare.Web.Models.ExpenseTracker;

namespace CloudCare.Web.Services.ExpenseTracker;

public class CategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }
    
    public async Task<List<Category>> GetCategoriesAsync()
    {
        return await _http.GetFromJsonAsync<List<Category>>("api/categories") 
               ?? new List<Category>();
    }
    
}