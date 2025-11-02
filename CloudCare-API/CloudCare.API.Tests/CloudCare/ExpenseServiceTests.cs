using CloudCare.API.Models;
using CloudCare.API.Repositories.EFCore;
using CloudCare.API.Repositories.Interfaces;
using CloudCare.API.Repositories.Mock;
using CloudCare.API.Services;
using Xunit;

namespace CloudCare.API.Tests.CloudCare;

public class ExpenseServiceTests
{
    private readonly MockExpenseRepository _repo;
    private readonly ExpenseService _service;

    public ExpenseServiceTests()
    {
        _repo = new MockExpenseRepository();
        _service = new ExpenseService(_repo);
    }

    [Fact]
    public void EnsureRecurringAsync_WithOnlyTemplateExpense_ReturnsTrue()
    {
 
        
        var result = _service.EnsureRecurringAsync(1);
        var value = result.Result;
        Assert.True(value);
    }
    
    [Fact]
    //this returns false becuse the expense is not over 30 days old yet. Its last month inclusive
    //meaning if today is aug 28, the expense is dated at july 29. and the billing cycle is monthly
    // this will not create a new expense until aug 30 or later
    public void EnsureRecurringAsync_WithTemplateAndAssociatedExpenses_ReturnsFalse()
    {
        // First expense (will create template and first instance)
        var firstExpense = new Expense
        {
            Description = "Monthly Internet Bill",
            Amount = 89.99m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(-2),
            IsRecurring = true,
            BillingCycle = BillingCycle.Monthly,
            CategoryId = 5,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 2,
            Notes = "Monthly Rogers Internet Bill"
        };
        _repo.AddExpenseAsync(firstExpense).Wait();

        // Second expense for next month
        var secondExpense = new Expense
        {
            Description = "Monthly Internet Bill",
            Amount = 89.99m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(-1),
            IsRecurring = true,
            BillingCycle = BillingCycle.Monthly,
            CategoryId = 5,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 2,
            Notes = "Monthly Rogers Internet Bill - Previous Month",
            RecurrenceSourceId = 1  // This will be set automatically by AddExpenseAsync
        };
        _repo.AddExpenseAsync(secondExpense).Wait();

        var result = _service.EnsureRecurringAsync(2);
        var value = result.Result;
        Assert.False(value);
    }
    
    [Fact]
    public void EnsureRecurringAsync_WithTemplateAndAssociatedExpenses_ReturnsTrue()
    {
        // First expense (will create template and first instance)
        var firstExpense = new Expense
        {
            Description = "Monthly Internet Bill",
            Amount = 89.99m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(-2),
            IsRecurring = true,
            BillingCycle = BillingCycle.Monthly,
            CategoryId = 5,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 2,
            Notes = "Monthly Rogers Internet Bill"
        };
        _repo.AddExpenseAsync(firstExpense).Wait();

        // Second expense for next month
        var secondExpense = new Expense
        {
            Description = "Monthly Internet Bill",
            Amount = 89.99m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddMonths(-1).AddDays(-1),
            IsRecurring = true,
            BillingCycle = BillingCycle.Monthly,
            CategoryId = 5,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 2,
            Notes = "Monthly Rogers Internet Bill - Previous Month",
            RecurrenceSourceId = 1  // This will be set automatically by AddExpenseAsync
        };
        _repo.AddExpenseAsync(secondExpense).Wait();

        var result = _service.EnsureRecurringAsync(2);
        var value = result.Result;
        Assert.True(value);
    }

    [Fact]
    public void EnsureRecurringAsync_WithNoRecurringExpenses_ReturnsFalse()
    {
        // Add a non-recurring expense
        var expense = new Expense
        {
            Description = "One-time Office Supplies",
            Amount = 150.75m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            IsRecurring = false,
            BillingCycle = BillingCycle.None,
            CategoryId = 6,
            VendorId = 4,
            PaymentMethodId = 1,
            UserId = 2,
            Notes = "One-time purchase"
        };
        _repo.AddExpenseAsync(expense).Wait();

        var result = _service.EnsureRecurringAsync(2);
        var value = result.Result;
        Assert.False(value);
    }
    
    //CHAT TESTS
    
    [Fact]
    public void EnsureRecurringAsync_WithMultipleTemplates_ReturnsTrue()
    {
        // Add first template and instance (Internet Bill)
        var internetExpense = new Expense
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
        };
        _repo.AddExpenseAsync(internetExpense).Wait();

        // Add second template and instance (Phone Bill)
        var phoneExpense = new Expense
        {
            Description = "Monthly Phone Bill",
            Amount = 75.00m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-30),
            IsRecurring = true,
            BillingCycle = BillingCycle.Monthly,
            CategoryId = 5,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 1,
            Notes = "Monthly Phone Bill"
        };
        _repo.AddExpenseAsync(phoneExpense).Wait();

        var result = _service.EnsureRecurringAsync(1);
        var value = result.Result;
        Assert.True(value);
    }

    [Fact]
    public void EnsureRecurringAsync_WithDifferentBillingCycles_ReturnsTrue()
    {
        // Weekly expense from 8 days ago (needs new instance)
        var weeklyExpense = new Expense
        {
            Description = "Weekly Team Lunch",
            Amount = 200.00m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-8),
            IsRecurring = true,
            BillingCycle = BillingCycle.Weekly,
            CategoryId = 1,
            VendorId = 1,
            PaymentMethodId = 1,
            UserId = 1,
            Notes = "Team lunch every week"
        };
        _repo.AddExpenseAsync(weeklyExpense).Wait();

        // Quarterly expense from 91 days ago (needs new instance)
        var quarterlyExpense = new Expense
        {
            Description = "Quarterly Insurance Premium",
            Amount = 500.00m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-91),
            IsRecurring = true,
            BillingCycle = BillingCycle.Quarterly,
            CategoryId = 10,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 1,
            Notes = "Business Insurance Premium"
        };
        _repo.AddExpenseAsync(quarterlyExpense).Wait();

        var result = _service.EnsureRecurringAsync(1);
        var value = result.Result;
        Assert.True(value);
    }

    [Fact]
    public void EnsureRecurringAsync_WithRecentlyCreatedExpenses_ReturnsFalse()
    {
        // Monthly expense from yesterday (shouldn't create new instance)
        var recentExpense = new Expense
        {
            Description = "Monthly Internet Bill",
            Amount = 89.99m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1),
            IsRecurring = true,
            BillingCycle = BillingCycle.Monthly,
            CategoryId = 5,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 66,
            Notes = "Monthly Rogers Internet Bill"
        };
        _repo.AddExpenseAsync(recentExpense).Wait();

        var result = _service.EnsureRecurringAsync(66);
        var value = result.Result;
        Assert.False(value);
    }

    [Fact]
    public void EnsureRecurringAsync_WithDifferentUsers_HandlesCorrectly()
    {
        // User 1's expense
        var user1Expense = new Expense
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
            Notes = "User 1's Internet Bill"
        };
        _repo.AddExpenseAsync(user1Expense).Wait();

        // User 2's expense (recent, shouldn't trigger new instance)
        var user2Expense = new Expense
        {
            Description = "Monthly Phone Bill",
            Amount = 75.00m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-1),
            IsRecurring = true,
            BillingCycle = BillingCycle.Monthly,
            CategoryId = 5,
            VendorId = 99,
            PaymentMethodId = 1,
            UserId = 2,
            Notes = "User 2's Phone Bill"
        };
        _repo.AddExpenseAsync(user2Expense).Wait();

        // Should create new instance for user 1 but not user 2
        var resultUser1 = _service.EnsureRecurringAsync(1);
        var resultUser2 = _service.EnsureRecurringAsync(2);
        
        Assert.True(resultUser1.Result);
        Assert.False(resultUser2.Result);
    }

    [Fact]
    public void EnsureRecurringAsync_WithBiWeeklyExpense_ReturnsTrue()
    {
        // Bi-weekly expense from 15 days ago (needs new instance)
        var biWeeklyExpense = new Expense
        {
            Description = "Bi-weekly Cleaning Service",
            Amount = 150.00m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-15),
            IsRecurring = true,
            BillingCycle = BillingCycle.BiWeekly,
            CategoryId = 4,
            VendorId = 8,
            PaymentMethodId = 1,
            UserId = 1,
            Notes = "Office cleaning every two weeks"
        };
        _repo.AddExpenseAsync(biWeeklyExpense).Wait();

        var result = _service.EnsureRecurringAsync(1);
        var value = result.Result;
        Assert.True(value);
    }

    [Fact]
    public void EnsureRecurringAsync_WithAnnualExpense_ReturnsTrue()
    {
        // Annual expense from 366 days ago (needs new instance)
        var annualExpense = new Expense
        {
            Description = "Annual Software License",
            Amount = 1200.00m,
            Date = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-366),
            IsRecurring = true,
            BillingCycle = BillingCycle.Annually,
            CategoryId = 15,
            VendorId = 2,
            PaymentMethodId = 1,
            UserId = 1,
            Notes = "Yearly software license renewal"
        };
        _repo.AddExpenseAsync(annualExpense).Wait();

        var result = _service.EnsureRecurringAsync(1);
        var value = result.Result;
        Assert.True(value);
    }
}