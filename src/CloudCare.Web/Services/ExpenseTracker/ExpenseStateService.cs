using CloudCare.Web.Models;
using Microsoft.Extensions.Logging;

namespace CloudCare.Web.Services.ExpenseTracker;

public class ExpenseStateService
{
    public ExpenseFormModel? ExpenseToProcess { get; private set; }
    private readonly ILogger<ExpenseStateService> _logger;

    public ExpenseStateService(ILogger<ExpenseStateService> logger)
    {
        _logger = logger;
    }

    public void SetExpense(ExpenseFormModel expense)
    {
        _logger.LogInformation("Setting expense to process: {ExpenseDescription}", expense.Description);
        ExpenseToProcess = expense;
    }

    public void ClearExpense()
    {
        _logger.LogInformation("Clearing expense to process.");
        ExpenseToProcess = null;
    }
}


