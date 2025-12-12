using CloudCare.Data.Models;

namespace CloudCare.Business.Repositories.Interfaces;

public interface IExpenseRepository
{
    Task<IEnumerable<Expense>> GetExpensesAsync(int userId);
    Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId);

    Task<int> AddExpenseAsync(Expense expense);
    Task<bool> UpdateExpenseAsync(Expense expense);
    Task<bool> DeleteExpenseAsync(int userId, int expenseId);

    //templates are like the blueprints for recurring expenses, so for example if you have a recurring expense for rent
    // you can create a template for it and then use that template to create the actual expense for rent every month

    Task<List<Expense>> GetRecurringTemplatesForUserAsync(int userId);

    Task<Expense?> GetExpenseByTemplateAndDateAsync(int userId, int templateId, DateOnly startDate, DateOnly endDate);
}
