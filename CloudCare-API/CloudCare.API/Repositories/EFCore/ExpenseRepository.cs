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
    public async Task<int> AddExpenseAsync(Expense expense)
    {
        _FinanceContext.Expenses.Add(expense);
        await _FinanceContext.SaveChangesAsync();
        return expense.Id; // EF CORE updates the entitys ID property after the save
    }


    public async Task<bool> DeleteExpenseAsync(int userId, int expenseId)
    {
        var expenseToDelete = await _FinanceContext.Expenses
            .FirstOrDefaultAsync(expense => expense.UserId == userId && expense.Id == expenseId);

        if (expenseToDelete == null)
            return false;

        _FinanceContext.Expenses.Remove(expenseToDelete);
        return await _FinanceContext.SaveChangesAsync() > 0;
    }

    public async Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId)
    {
        return await _FinanceContext.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Vendor)
            .Include(e => e.PaymentMethod)
            .Where(expense => expense.Id == expenseId && expense.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync(int userId)
    {
        return await _FinanceContext.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Vendor)
            .Include(e => e.PaymentMethod)
            .Where(e => e.UserId == userId)
            .OrderByDescending(c => c.Id)
            .ToListAsync();
    }

    public async Task<bool> UpdateExpenseAsync(Expense expense)
    {
        _FinanceContext.Expenses.Update(expense);
        return await _FinanceContext.SaveChangesAsync() > 0;
    }
}
