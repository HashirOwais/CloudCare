using System;
using CloudCare.API.DbContexts;
using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.API.Repositories.EFCore;

public class ExpenseRepository : IExpenseRepository
{
    public readonly FinanceContext _FinanceContext;

    public ExpenseRepository(FinanceContext financeContext)
    {
        _FinanceContext = financeContext ?? throw new ArgumentNullException(nameof(financeContext));
    }


    public async Task AddExpenseAsync(Expense expense)
    {
        throw new NotImplementedException();
 

    }

    public Task DeleteExpenseAsync(int userId, int expenseId)
    {
        throw new NotImplementedException();
    }

    public Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync(int userId)
    {
        return await _FinanceContext.Expenses.AsNoTracking().Where(e => e.UserId == userId).ToListAsync();

    }

    public Task UpdateExpenseAsync(Expense expense)
    {
        throw new NotImplementedException();
    }
}
