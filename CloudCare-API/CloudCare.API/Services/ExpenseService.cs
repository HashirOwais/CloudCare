namespace CloudCare.API.Services;

public class ExpenseService : IExpenseService
{
    public async Task<bool> EnsureRecurringAsync(int? userId = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}