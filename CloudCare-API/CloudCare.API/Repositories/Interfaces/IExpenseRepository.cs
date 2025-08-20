using CloudCare.API.Models;

namespace CloudCare.API.Repositories.Interfaces;

public interface IExpenseRepository
{
    Task<IEnumerable<Expense>> GetExpensesAsync(int userId);
    Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId);

    Task<int> AddExpenseAsync(Expense expense);
    Task<bool> UpdateExpenseAsync(Expense expense);
    Task <bool> DeleteExpenseAsync(int userId, int expenseId);
    
    //Task<IEnumerable<Expense>> GetAllIsReoccurringAsync(int userId);
}