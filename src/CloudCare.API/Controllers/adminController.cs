using CloudCare.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[Authorize]
[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IExpenseService _expenseService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IExpenseService expenseService, ILogger<AdminController> logger)
    {
        _expenseService = expenseService;
        _logger = logger;
    }

    [HttpPost("ensure-recurring")]
    public async Task<IActionResult> EnsureRecurring([FromQuery] int userId)
    {
        _logger.LogInformation("EnsureRecurring called for userId: {userId}", userId);
        var result = await _expenseService.EnsureRecurringAsync(userId);
        _logger.LogInformation("EnsureRecurring finished for userId: {userId} with result: {result}", userId, result);
        return Ok(new { success = result });
    }
}
