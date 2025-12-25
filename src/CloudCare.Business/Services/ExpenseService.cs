using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace CloudCare.Business.Services;

public class ExpenseService : IExpenseService
{
    //the reason why we make the exp repo the type Iexp repo is becuase this allows to inject any implementation of I exp repo
    // for ex, there is mockExp repo that implememnts the I exp repo so that i can inject. 
    // there is also the efcore exp repo that implements the I exp repo. 

    private readonly IExpenseRepository _expenseRepository;
    private readonly ILogger<ExpenseService> _logger;

    public ExpenseService(IExpenseRepository repository, ILogger<ExpenseService> logger)
    {
        _expenseRepository = repository;
        _logger = logger;
    }
    public async Task<bool> EnsureRecurringAsync(int userId)
    {
        _logger.LogInformation("Starting to ensure recurring expenses for user {UserId}", userId);
        bool result = false;
        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        DateOnly sevenDaysAgo = today.AddDays(-7);
        DateOnly fourteenDaysAgo = today.AddDays(-14);
        DateOnly thirtyDaysAgo = today.AddMonths(-1);
        DateOnly ninetyDaysAgo = today.AddMonths(-3);
        DateOnly oneHundredEightyDaysAgo = today.AddMonths(-6);
        DateOnly oneYearAgo = today.AddYears(-1);

        var templates = await _expenseRepository.GetRecurringTemplatesForUserAsync(userId);
        _logger.LogInformation("Found {TemplateCount} recurring templates for user {UserId}", templates.Count(), userId);

        foreach (var template in templates)
        {
            _logger.LogInformation("Processing template {TemplateId} for user {UserId}", template.Id, userId);
            int cycle = (int)template.BillingCycle;
            Expense? existingExpense = null;

            switch (cycle)
            {
                case 0:
                    _logger.LogInformation("Template {TemplateId} has a billing cycle of 0, skipping.", template.Id);
                    continue;
                case 1:
                    existingExpense = await _expenseRepository.GetExpenseByTemplateAndDateAsync(userId, template.Id, sevenDaysAgo, today);
                    break;
                case 2:
                    existingExpense = await _expenseRepository.GetExpenseByTemplateAndDateAsync(userId, template.Id, fourteenDaysAgo, today);
                    break;
                case 3:
                    existingExpense = await _expenseRepository.GetExpenseByTemplateAndDateAsync(userId, template.Id, thirtyDaysAgo, today);
                    break;
                case 4:
                    existingExpense = await _expenseRepository.GetExpenseByTemplateAndDateAsync(userId, template.Id, ninetyDaysAgo, today);
                    break;
                case 5:
                    existingExpense = await _expenseRepository.GetExpenseByTemplateAndDateAsync(userId, template.Id, oneHundredEightyDaysAgo, today);
                    break;
                case 6:
                    existingExpense = await _expenseRepository.GetExpenseByTemplateAndDateAsync(userId, template.Id, oneYearAgo, today);
                    break;
                default:
                    _logger.LogWarning("Template {TemplateId} has an unknown billing cycle {BillingCycle}, skipping.", template.Id, cycle);
                    continue;
            }

            if (existingExpense == null)
            {
                _logger.LogInformation("No existing expense found for template {TemplateId}, creating a new one.", template.Id);
                //create new expense based on template

                var expense = new Expense
                {
                    UserId = template.UserId,
                    Amount = template.Amount,
                    Description = $"{template.Description} for {today:MMMM yyyy}",
                    CategoryId = template.CategoryId,
                    VendorId = template.VendorId,
                    PaymentMethodId = template.PaymentMethodId,
                    Date = today,
                    IsRecurring = true,
                    RecurrenceSourceId = template.Id,
                    Notes = template.Notes,
                    ReceiptUrl = template.ReceiptUrl,
                    BillingCycle = template.BillingCycle

                };
                await _expenseRepository.AddExpenseAsync(expense);
                _logger.LogInformation("Successfully created a new expense from template {TemplateId}", template.Id);
                result = true;
            }
            else
            {
                _logger.LogInformation("Existing expense found for template {TemplateId}, no action needed.", template.Id);
            }

        }

        _logger.LogInformation("Finished ensuring recurring expenses for user {UserId} with result {Result}", userId, result);
        return result;
    }
}
