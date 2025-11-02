using CloudCare.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public AdminController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    [HttpPost("ensure-recurring")]
    public async Task<IActionResult> EnsureRecurring([FromQuery] int userId)
    {
        var result = await _expenseService.EnsureRecurringAsync(userId);
        return Ok(new { success = result });
    }
}
