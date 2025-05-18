using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;

namespace CloudCare.API.Repositories.Mock;

public class MockExpenseRepository : IExpenseRepository
{
    private readonly List<Category> _categories = new()
    {
        new Category { Id = 1, Name = "Food" },
        new Category { Id = 2, Name = "Supplies" },
        new Category { Id = 3, Name = "Transportation" }
    };

    private readonly List<Vendor> _vendors = new()
    {
        new Vendor { Id = 1, Name = "Walmart" },
        new Vendor { Id = 2, Name = "Amazon" },
        new Vendor { Id = 3, Name = "Joe's Print Shop" }
    };

    private readonly List<PaymentMethod> _paymentMethods = new()
    {
        new PaymentMethod { Id = 1, Name = "Cash" },
        new PaymentMethod { Id = 2, Name = "Credit Card" },
        new PaymentMethod { Id = 3, Name = "E-Transfer" }
    };

    private readonly List<Expense> _expenses;

    public MockExpenseRepository()
    {
        _expenses = new List<Expense>
        {
            new Expense
            {
                Id = 1,
                UserId = 1,
                Description = "Snacks",
                Amount = 20.00m,
                Date = DateTime.Today,
                CategoryId = 1,
                VendorId = 1,
                PaymentMethodId = 1,
                IsRecurring = false
            },
            new Expense
            {
                Id = 2,
                UserId = 1,
                Description = "Toys",
                Amount = 35.00m,
                Date = DateTime.Today.AddDays(-2),
                CategoryId = 2,
                VendorId = 2,
                PaymentMethodId = 2,
                IsRecurring = true
            },
            new Expense
            {
                Id = 3,
                UserId = 1,
                Description = "Field Trip Supplies",
                Amount = 50.00m,
                Date = DateTime.Today.AddDays(-5),
                CategoryId = 2,
                VendorId = 3,
                PaymentMethodId = 1,
                IsRecurring = false
            },
            new Expense
            {
                Id = 4,
                UserId = 2,
                Description = "Art Supplies",
                Amount = 15.00m,
                Date = DateTime.Today.AddDays(-1),
                CategoryId = 2,
                VendorId = 1,
                PaymentMethodId = 1,
                IsRecurring = false
            },
            new Expense
            {
                Id = 5,
                UserId = 2,
                Description = "Lunch for Staff",
                Amount = 45.00m,
                Date = DateTime.Today.AddDays(-3),
                CategoryId = 1,
                VendorId = 2,
                PaymentMethodId = 3,
                IsRecurring = false
            },
            new Expense
            {
                Id = 6,
                UserId = 2,
                Description = "Monthly Cleaning Service",
                Amount = 120.00m,
                Date = DateTime.Today.AddDays(-30),
                CategoryId = 3,
                VendorId = 3,
                PaymentMethodId = 2,
                IsRecurring = true
            }
        };

        // Populate navigation properties
        foreach (var expense in _expenses)
        {
            expense.Category = _categories.First(c => c.Id == expense.CategoryId);
            expense.Vendor = _vendors.First(v => v.Id == expense.VendorId);
            expense.PaymentMethod = _paymentMethods.First(p => p.Id == expense.PaymentMethodId);
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