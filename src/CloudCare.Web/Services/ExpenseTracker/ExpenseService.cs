using System.Net.Http.Json;
using CloudCare.Shared.DTOs.ExpenseTracker;

namespace CloudCare.Web.Services.ExpenseTracker;

public class ExpenseService
{
    private readonly HttpClient _http;

    public ExpenseService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ReadExpenseDto>> GetExpensesAsync()
    {
        return await _http.GetFromJsonAsync<List<ReadExpenseDto>>("api/expenses") 
               ?? new List<ReadExpenseDto>();
    }
    
    public async Task<ReadExpenseDto?> PostExpenseAsync(CreateExpenseDto newExpense)
    {
        var response = await _http.PostAsJsonAsync("api/expenses", newExpense);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ReadExpenseDto>();
        }
        return null;
    }

    public async Task<ReadExpenseDto?> UpdateExpenseAsync(ReadExpenseDto expense)
    {
        var response = await _http.PutAsJsonAsync($"api/expenses/{expense.Id}", expense);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReadExpenseDto>();
    }

    public async Task DeleteExpenseAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/expenses/{id}");
        response.EnsureSuccessStatusCode();
    }
}