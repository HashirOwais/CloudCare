using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockExpenseRepository : IExpenseRepository
{
    private readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Food & Snacks" },
        new Category { Id = 2, Name = "Educational Supplies" },
        new Category { Id = 3, Name = "Toys & Games" },
        new Category { Id = 4, Name = "Cleaning Supplies" },
        new Category { Id = 5, Name = "Utilities" },
        new Category { Id = 6, Name = "Office Supplies" },
        new Category { Id = 7, Name = "Furniture & Fixtures" },
        new Category { Id = 8, Name = "Repairs & Maintenance" },
        new Category { Id = 9, Name = "Transportation" },
        new Category { Id = 10, Name = "Insurance" },
        new Category { Id = 11, Name = "Professional Services" },
        new Category { Id = 12, Name = "Marketing & Advertising" },
        new Category { Id = 13, Name = "Staff Wages" },
        new Category { Id = 14, Name = "Training & Development" },
        new Category { Id = 15, Name = "Licenses & Permits" },
        new Category { Id = 99, Name = "Miscellaneous" }
    };
    private readonly List<Vendor> _vendors = new()
    {
        new Vendor { Id = 1, Name = "Walmart" },
        new Vendor { Id = 2, Name = "Amazon" },
        new Vendor { Id = 3, Name = "Costco" },
        new Vendor { Id = 4, Name = "Staples" },
        new Vendor { Id = 5, Name = "Home Depot" },
        new Vendor { Id = 6, Name = "Best Buy" },
        new Vendor { Id = 7, Name = "Private Marketplace" },
        new Vendor { Id = 8, Name = "Local Vendor" },
        new Vendor { Id = 9, Name = "Government" },
        new Vendor { Id = 99, Name = "Miscellaneous" }
    };

    private readonly List<PaymentMethod> _paymentMethods = new()
    {
        new PaymentMethod { Id = 1, Name = "Credit Card" },
        new PaymentMethod { Id = 2, Name = "Debit Card" },
        new PaymentMethod { Id = 3, Name = "Cash" },
        new PaymentMethod { Id = 4, Name = "E-Transfer" },
        new PaymentMethod { Id = 5, Name = "Cheque" }
    };

    private readonly List<Expense> _expenses;

    public MockExpenseRepository()
    {
        _expenses = new List<Expense>
        {
            new Expense { Id = 1, UserId = 1, Description = "Snacks for kids", Amount = 20.00m, Date = DateTime.Today, CategoryId = 1, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
            new Expense { Id = 2, UserId = 1, Description = "Toys purchase", Amount = 35.00m, Date = DateTime.Today.AddDays(-2), CategoryId = 3, VendorId = 2, PaymentMethodId = 2, IsRecurring = true },
            new Expense { Id = 3, UserId = 1, Description = "Field Trip Supplies", Amount = 50.00m, Date = DateTime.Today.AddDays(-5), CategoryId = 2, VendorId = 3, PaymentMethodId = 1, IsRecurring = false },
            new Expense { Id = 4, UserId = 1, Description = "Office Supplies restock", Amount = 22.50m, Date = DateTime.Today.AddDays(-6), CategoryId = 6, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
            new Expense { Id = 5, UserId = 1, Description = "Books for reading time", Amount = 80.00m, Date = DateTime.Today.AddDays(-7), CategoryId = 2, VendorId = 2, PaymentMethodId = 2, IsRecurring = false },
            new Expense { Id = 6, UserId = 1, Description = "Monthly Cleaning Service", Amount = 120.00m, Date = DateTime.Today.AddDays(-10), CategoryId = 4, VendorId = 3, PaymentMethodId = 3, IsRecurring = true },
            new Expense { Id = 7, UserId = 1, Description = "Birthday Party expenses", Amount = 150.00m, Date = DateTime.Today.AddDays(-15), CategoryId = 1, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
            new Expense { Id = 8, UserId = 1, Description = "Printer Ink replacement", Amount = 60.00m, Date = DateTime.Today.AddDays(-20), CategoryId = 6, VendorId = 2, PaymentMethodId = 2, IsRecurring = false },
            new Expense { Id = 9, UserId = 1, Description = "Online Workshop for Staff", Amount = 95.00m, Date = DateTime.Today.AddDays(-22), CategoryId = 14, VendorId = 3, PaymentMethodId = 1, IsRecurring = false },
            new Expense { Id = 10, UserId = 1, Description = "Weekly Snacks", Amount = 25.00m, Date = DateTime.Today.AddDays(-3), CategoryId = 1, VendorId = 1, PaymentMethodId = 3, IsRecurring = true },
            new Expense { Id = 11, UserId = 1, Description = "Monthly Software Subscription", Amount = 45.99m, Date = DateTime.Today.AddDays(-29), CategoryId = 12, VendorId = 2, PaymentMethodId = 2, IsRecurring = true },

            // User 2
            new Expense { Id = 12, UserId = 2, Description = "Art Supplies", Amount = 15.00m, Date = DateTime.Today.AddDays(-1), CategoryId = 2, VendorId = 1, PaymentMethodId = 1, IsRecurring = false },
            new Expense { Id = 13, UserId = 2, Description = "Lunch for Staff", Amount = 45.00m, Date = DateTime.Today.AddDays(-3), CategoryId = 1, VendorId = 2, PaymentMethodId = 3, IsRecurring = false },
            new Expense { Id = 14, UserId = 2, Description = "Monthly Cleaning Service", Amount = 120.00m, Date = DateTime.Today.AddDays(-30), CategoryId = 4, VendorId = 3, PaymentMethodId = 2, IsRecurring = true }
        };

        // Set navigation properties
        foreach (var expense in _expenses)
        {
            expense.Category = _categories.FirstOrDefault(c => c.Id == expense.CategoryId);
            expense.Vendor = _vendors.FirstOrDefault(v => v.Id == expense.VendorId);
            expense.PaymentMethod = _paymentMethods.FirstOrDefault(p => p.Id == expense.PaymentMethodId);
        }
    }

    public Task<IEnumerable<Expense>> GetExpensesAsync(int userId)
    {
        var result = _expenses
            .Where(e => e.UserId == userId)
            .ToList();

        return Task.FromResult(result.AsEnumerable());
    }

    public Task<Expense?> GetExpenseByIdAsync(int userId, int expenseId)
    {
        var result = _expenses.FirstOrDefault(e => e.UserId == userId && e.Id == expenseId);
        return Task.FromResult(result);
    }

    public Task AddExpenseAsync(Expense expense)
    {
        expense.Id = _expenses.Max(e => e.Id) + 1;
        expense.Category = _categories.First(c => c.Id == expense.CategoryId);
        expense.Vendor = _vendors.First(v => v.Id == expense.VendorId);
        expense.PaymentMethod = _paymentMethods.First(p => p.Id == expense.PaymentMethodId);

        _expenses.Add(expense);
        return Task.CompletedTask;
    }

    public Task UpdateExpenseAsync(Expense expense)
    {
        var index = _expenses.FindIndex(e => e.UserId == expense.UserId && e.Id == expense.Id);
        if (index != -1)
        {
            expense.Category = _categories.First(c => c.Id == expense.CategoryId);
            expense.Vendor = _vendors.First(v => v.Id == expense.VendorId);
            expense.PaymentMethod = _paymentMethods.First(p => p.Id == expense.PaymentMethodId);

            _expenses[index] = expense;
        }

        return Task.CompletedTask;
    }

    public Task DeleteExpenseAsync(int userId, int expenseId)
    {
        var expense = _expenses.FirstOrDefault(e => e.UserId == userId && e.Id == expenseId);
        if (expense != null)
        {
            _expenses.Remove(expense);
        }

        return Task.CompletedTask;
    }
}