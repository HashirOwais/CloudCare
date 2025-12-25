using System.Net.Http.Json;
using CloudCare.Shared.DTOs.ExpenseTracker;
using Microsoft.Extensions.Logging;

namespace CloudCare.Web.Services.ExpenseTracker;

public class ExpenseService
{
    private readonly HttpClient _http;
    private readonly ILogger<ExpenseService> _logger;

    public ExpenseService(HttpClient http, ILogger<ExpenseService> logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<List<ReadExpenseDto>> GetExpensesAsync()
    {
        _logger.LogInformation("Fetching expenses from API.");
        try
        {
            var expenses = await _http.GetFromJsonAsync<List<ReadExpenseDto>>("api/expenses");
            _logger.LogInformation("Successfully fetched {Count} expenses.", expenses?.Count ?? 0);
            return expenses ?? new List<ReadExpenseDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching expenses from API.");
            return new List<ReadExpenseDto>();
        }
    }
    
    public async Task<ReadExpenseDto?> PostExpenseAsync(ExpenseForCreationDto newExpense)
    {
        _logger.LogInformation("Creating new expense.");
        try
        {
            var response = await _http.PostAsJsonAsync("api/expenses", newExpense);
            if (response.IsSuccessStatusCode)
            {
                var createdExpense = await response.Content.ReadFromJsonAsync<ReadExpenseDto>();
                _logger.LogInformation("Successfully created new expense with ID: {ExpenseId}", createdExpense?.Id);
                return createdExpense;
            }
            else
            {
                _logger.LogError("Error creating new expense. Status code: {StatusCode}", response.StatusCode);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating new expense.");
            return null;
        }
    }

    public async Task UpdateExpenseAsync(int id, ExpenseForUpdateDto expense)
    {
        _logger.LogInformation("Updating expense with ID: {ExpenseId}", id);
        try
        {
            var response = await _http.PutAsJsonAsync($"api/expenses/{id}", expense);
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Successfully updated expense with ID: {ExpenseId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating expense with ID: {ExpenseId}", id);
        }
    }

    public async Task DeleteExpenseAsync(int id)
    {
        _logger.LogInformation("Deleting expense with ID: {ExpenseId}", id);
        try
        {
            var response = await _http.DeleteAsync($"api/expenses/{id}");
            response.EnsureSuccessStatusCode();
            _logger.LogInformation("Successfully deleted expense with ID: {ExpenseId}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting expense with ID: {ExpenseId}", id);
        }
    }
}