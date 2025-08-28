namespace CloudCare.API.Services;

public interface IExpenseService
{
    Task<bool> EnsureRecurringAsync(int? userId = null, CancellationToken ct = default);
}