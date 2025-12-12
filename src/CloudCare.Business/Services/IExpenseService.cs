using CloudCare.Data.Models;

namespace CloudCare.Business.Services;

public interface IExpenseService
{
    Task<bool> EnsureRecurringAsync(int userId);

    //Task<int> AddExpenseAsync(Expense expense); implement this logic later 
}
