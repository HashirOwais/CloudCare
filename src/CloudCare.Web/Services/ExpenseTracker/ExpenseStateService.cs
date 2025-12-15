using CloudCare.Web.Models;

namespace CloudCare.Web.Services.ExpenseTracker;

public class ExpenseStateService
{
    public ExpenseFormModel? ExpenseToProcess { get; private set; }

    public void SetExpense(ExpenseFormModel expense)
    {
        ExpenseToProcess = expense;
    }

    public void ClearExpense()
    {
        ExpenseToProcess = null;
    }
}


