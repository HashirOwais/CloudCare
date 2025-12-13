using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;

namespace CloudCare.Business.Repositories.Mock;

public class MockExpenseRepository : IExpenseRepository
{
    // In-memory backing store
    private readonly List<Expense> _expenses = new();
    private readonly List<Category> _categories = new();
    private readonly List<Vendor> _vendors = new();
    private readonly List<PaymentMethod> _paymentMethods = new();

    public MockExpenseRepository()
    {
        // Seed Categories
        _categories.AddRange(new[]
        {
            new Category { Id = 1, Name = "Food & Snacks" },
            new Category { Id = 2, Name = "Educational Supplies" },
            new Category { Id = 3, Name = "Toys & Games" },
            new Category { Id = 4, Name = "Cleaning Supplies" },
            new Category { Id = 5, Name = "Utilities" },
            new Category { Id = 6, Name = "Office Supplies" },
            new Category { Id = 99, Name = "Miscellaneous" }
        });

        // Seed Vendors
        _vendors.AddRange(new[]
        {
            new Vendor { Id = 1, Name = "Walmart" },
            new Vendor { Id = 2, Name = "Amazon" },
            new Vendor { Id = 3, Name = "Costco" },
            new Vendor { Id = 4, Name = "Staples" },
            new Vendor { Id = 99, Name = "Miscellaneous" }
        });

        // Seed Payment Methods
        _paymentMethods.AddRange(new[]
        {
            new PaymentMethod { Id = 1, Name = "Credit Card" },
            new PaymentMethod { Id = 2, Name = "Debit Card" },
            new PaymentMethod { Id = 3, Name = "Cash" },
            new PaymentMethod { Id = 4, Name = "E-Transfer" }
        });

        _expenses.AddRange(new[]
            {
                new Expense
                {
                    Description = "Monthly Internet Bill",
                    Amount = 89.99m,
                    Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-30),
                    IsRecurring = true,
                    BillingCycle = BillingCycle.Monthly,
                    CategoryId = 5,
                    VendorId = 99,
                    PaymentMethodId = 1,
                    UserId = 1,
                    Notes = "Monthly Rogers Internet Bill"
                }
            }
        );
    }

    public Task<IEnumerable<Expense>> GetExpensesAsync(int userId)
    {
        var userExpenses = _expenses.Where(e => e.UserId == userId &&
            (e.RecurrenceSourceId != null || !e.IsRecurring));
        return Task.FromResult(userExpenses.AsEnumerable());
    }

    public Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId)
    {
        var expense = _expenses.FirstOrDefault(e => e.UserId == userId && e.Id == expenseId);
        return Task.FromResult(expense);
    }

    public Task<int> AddExpenseAsync(Expense expense)
    {
        var newId = _expenses.Any() ? _expenses.Max(e => e.Id) + 1 : 1;

        if (expense.IsRecurring && expense.RecurrenceSourceId == null)
        {
            // Create template first
            var template = new Expense
            {
                Id = newId,
                UserId = expense.UserId,
                Amount = expense.Amount,
                Description = expense.Description,
                CategoryId = expense.CategoryId,
                VendorId = expense.VendorId,
                PaymentMethodId = expense.PaymentMethodId,
                Date = expense.Date,
                IsRecurring = true,
                RecurrenceSourceId = null,
                Notes = expense.Notes,
                ReceiptUrl = expense.ReceiptUrl,
                BillingCycle = expense.BillingCycle
            };

            _expenses.Add(template);

            // Update the newId for the actual expense
            newId = _expenses.Max(e => e.Id) + 1;
            expense.RecurrenceSourceId = template.Id;
        }

        expense.Id = newId;
        _expenses.Add(expense);
        return Task.FromResult(newId);
    }

    public Task<bool> UpdateExpenseAsync(Expense expense)
    {
        var existing = _expenses.FirstOrDefault(e => e.Id == expense.Id && e.UserId == expense.UserId);
        if (existing == null) return Task.FromResult(false);

        // Update fields
        existing.Amount = expense.Amount;
        existing.Description = expense.Description;
        existing.CategoryId = expense.CategoryId;
        existing.VendorId = expense.VendorId;
        existing.PaymentMethodId = expense.PaymentMethodId;
        existing.Date = expense.Date;
        existing.IsRecurring = expense.IsRecurring;
        existing.Notes = expense.Notes;
        existing.ReceiptUrl = expense.ReceiptUrl;

        return Task.FromResult(true);
    }

    public Task<bool> DeleteExpenseAsync(int userId, int expenseId)
    {
        var existing = _expenses.FirstOrDefault(e => e.UserId == userId && e.Id == expenseId);
        if (existing == null) return Task.FromResult(false);

        _expenses.Remove(existing);
        return Task.FromResult(true);
    }

    public async Task<List<Expense>> GetRecurringTemplatesForUserAsync(int userId)
    {
        var templates = _expenses
            .Where(e => e.UserId == userId && e.IsRecurring && e.RecurrenceSourceId == null)
            .ToList();
        return await Task.FromResult(templates);
    }

    public async Task<Expense?> GetExpenseByTemplateAndDateAsync(int userId, int templateId, DateOnly startDate, DateOnly endDate)
    {
        var expense = _expenses.FirstOrDefault(e =>
            e.UserId == userId &&
            e.RecurrenceSourceId == templateId &&
            e.Date >= startDate &&
            e.Date <= endDate);
        return await Task.FromResult(expense);
    }
}