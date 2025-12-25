using CloudCare.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        _logger.LogInformation("Ensuring recurring expenses for user {UserId}", userId);
        var result = await _expenseService.EnsureRecurringAsync(userId);
        _logger.LogInformation("Successfully ensured recurring expenses for user {UserId}", userId);
        return Ok(new { success = result });
    }
}
