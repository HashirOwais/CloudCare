using AutoMapper;
using CloudCare.Shared.DTOs.ExpenseTracker;
using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using CloudCare.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[Authorize]
[ApiController]
[Route("/api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public ExpensesController(
        IExpenseRepository expenseRepository,
        IMapper mapper,
        IUserService userService
    )
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadExpenseDto>>> GetAllExpenses()
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var expenses = await _expenseRepository.GetExpensesAsync(userId.Value);
        var expenseDtos = _mapper.Map<IEnumerable<ReadExpenseDto>>(expenses);
        return Ok(expenseDtos);
    }

    [HttpGet("{expenseId}")]
    public async Task<ActionResult<ReadExpenseDto>> GetExpenseById(int expenseId)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var expense = await _expenseRepository.GetExpenseByIdAsync(userId.Value, expenseId);
        if (expense == null)
            return NotFound();

        var readDto = _mapper.Map<ReadExpenseDto>(expense);
        return Ok(readDto);
    }

    [HttpPost]
    public async Task<ActionResult<ReadExpenseDto>> CreateExpense([FromBody] ExpenseForCreationDto dto)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var expense = _mapper.Map<Expense>(dto);
        expense.UserId = userId.Value;

        var newId = await _expenseRepository.AddExpenseAsync(expense);
        if (newId == 0)
            return BadRequest("Could not create expense.");

        var newExpense = await _expenseRepository.GetExpenseByIdAsync(expense.UserId, newId);
        if (newExpense == null)
            return NotFound("Expense created, but not found on fetch.");

        var readDto = _mapper.Map<ReadExpenseDto>(newExpense);
        return CreatedAtAction(nameof(GetExpenseById), new { expenseId = newId }, readDto);
    }

    [HttpPut("{expenseId}")]
    public async Task<ActionResult> UpdateExpense(int expenseId, [FromBody] ExpenseForUpdateDto dto)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var expense = await _expenseRepository.GetExpenseByIdAsync(userId.Value, expenseId);
        if (expense == null)
        {
            return NotFound();
        }

        _mapper.Map(dto, expense);
        await _expenseRepository.UpdateExpenseAsync(expense);
        return NoContent();
    }

    [HttpDelete("{expenseId}")]
    public async Task<ActionResult> DeleteExpense(int expenseId)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var expense = await _expenseRepository.GetExpenseByIdAsync(userId.Value, expenseId);
        if (expense == null)
        {
            return NotFound();
        }

        await _expenseRepository.DeleteExpenseAsync(userId.Value, expenseId);
        return NoContent();
    }
}
