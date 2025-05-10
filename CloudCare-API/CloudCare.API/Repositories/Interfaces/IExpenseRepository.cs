using CloudCare.API.Models;

namespace CloudCare.API.Repositories.Interfaces;

public interface IExpenseRepository
{
    Task<IEnumerable<Expense>> GetExpensesAsync(int userId);
    Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId);

    Task AddExpenseAsync(Expense expense);
    Task UpdateExpenseAsync(Expense expense);
    Task DeleteExpenseAsync(int userId, int expenseId);
}