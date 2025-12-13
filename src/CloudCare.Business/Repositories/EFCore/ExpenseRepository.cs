using System;
using CloudCare.Data.DbContexts;
using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CloudCare.Business.Repositories.EFCore;

public class ExpenseRepository : IExpenseRepository
{
    public readonly CloudCareContext _cloudCareContext;

    public ExpenseRepository(CloudCareContext cloudCareContext)
    {
        _cloudCareContext = cloudCareContext ?? throw new ArgumentNullException(nameof(CloudCareContext));
    }

    public async Task<int> AddExpenseAsync(Expense expense)
    {
        //TODO: add this add expense logic to the expense service later
        if (expense.IsRecurring && expense.RecurrenceSourceId == null)
        {
            // This is a new recurring expense - create template first
            var template = new Expense
            {
                UserId = expense.UserId,
                Amount = expense.Amount,
                Description = expense.Description,
                CategoryId = expense.CategoryId,
                VendorId = expense.VendorId,
                PaymentMethodId = expense.PaymentMethodId,
                Date = expense.Date,
                IsRecurring = true,
                RecurrenceSourceId = null, // Template has no source
                Notes = expense.Notes,
                ReceiptUrl = expense.ReceiptUrl,
                BillingCycle = expense.BillingCycle
            };

            _cloudCareContext.Expenses.Add(template);
            await _cloudCareContext.SaveChangesAsync();

            // Set the template as the source for the actual expense
            expense.RecurrenceSourceId = template.Id;
        }

        // Add the actual expense
        _cloudCareContext.Expenses.Add(expense);
        await _cloudCareContext.SaveChangesAsync();
        return expense.Id;
    }

    public async Task<bool> DeleteExpenseAsync(int userId, int expenseId)
    {
        var expenseToDelete = await _cloudCareContext.Expenses
            .FirstOrDefaultAsync(expense => expense.UserId == userId && expense.Id == expenseId);

        if (expenseToDelete == null)
            return false;

        _cloudCareContext.Expenses.Remove(expenseToDelete);
        return await _cloudCareContext.SaveChangesAsync() > 0;
    }

    public Task<List<Expense>> GetRecurringTemplatesForUserAsync(int userId)
    {
        return _cloudCareContext.Expenses
            .AsNoTracking()
            .Where(e => e.UserId == userId && e.IsRecurring && e.RecurrenceSourceId == null)
            .OrderBy(e => e.Id)
            .ToListAsync();
    }

    public async Task<Expense?> GetExpenseByTemplateAndDateAsync(int userId, int templateId, DateOnly startDate, DateOnly endDate)
    {
        return await _cloudCareContext.Expenses
            .FirstOrDefaultAsync(e =>
                e.UserId == userId &&
                e.RecurrenceSourceId == templateId &&
                e.Date >= startDate &&
                e.Date <= endDate);
    }

    public async Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId)
    {
        return await _cloudCareContext.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Vendor)
            .Include(e => e.PaymentMethod)
            .Where(expense => expense.Id == expenseId && expense.UserId == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Expense>> GetExpensesAsync(int userId)
    {
        return await _cloudCareContext.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .Include(e => e.Vendor)
            .Include(e => e.PaymentMethod)
            .Where(e => e.UserId == userId && (e.RecurrenceSourceId != null || !e.IsRecurring))
            .OrderByDescending(c => c.Date)
            .ToListAsync();
    }

    public async Task<bool> UpdateExpenseAsync(Expense expense)
    {
        _cloudCareContext.Expenses.Update(expense);
        return await _cloudCareContext.SaveChangesAsync() > 0;
    }
}