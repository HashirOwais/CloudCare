using CloudCare.API.Models;

namespace CloudCare.API.Services;

public interface IExpenseService
{
    Task<bool> EnsureRecurringAsync(int userId);

    //Task<int> AddExpenseAsync(Expense expense); implement this logic later 
}
