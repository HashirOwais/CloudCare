using AutoMapper;
using CloudCare.Shared.DTOs.ExpenseTracker;
using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using CloudCare.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudCare.API.Controllers;

[Authorize]
[ApiController]
[Route("/api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ILogger<ExpensesController> _logger;

    public ExpensesController(
        IExpenseRepository expenseRepository,
        IMapper mapper,
        IUserService userService,
        ILogger<ExpensesController> logger
    )
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadExpenseDto>>> GetAllExpenses()
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt to get all expenses.");
            return Unauthorized();
        }
        
        _logger.LogInformation("User {UserId} is getting all expenses.", userId);
        var expenses = await _expenseRepository.GetExpensesAsync(userId.Value);
        var expenseDtos = _mapper.Map<IEnumerable<ReadExpenseDto>>(expenses);
        _logger.LogInformation("Successfully retrieved all expenses for user {UserId}.", userId);
        return Ok(expenseDtos);
    }

    [HttpGet("{expenseId}")]
    public async Task<ActionResult<ReadExpenseDto>> GetExpenseById(int expenseId)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt to get expense {ExpenseId}.", expenseId);
            return Unauthorized();
        }

        _logger.LogInformation("User {UserId} is getting expense {ExpenseId}.", userId, expenseId);
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId.Value, expenseId);
        if (expense == null)
        {
            _logger.LogWarning("Expense {ExpenseId} not found for user {UserId}.", expenseId, userId);
            return NotFound();
        }

        var readDto = _mapper.Map<ReadExpenseDto>(expense);
        _logger.LogInformation("Successfully retrieved expense {ExpenseId} for user {UserId}.", expenseId, userId);
        return Ok(readDto);
    }

    [HttpPost]
    public async Task<ActionResult<ReadExpenseDto>> CreateExpense([FromBody] ExpenseForCreationDto dto)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt to create an expense.");
            return Unauthorized();
        }

        _logger.LogInformation("User {UserId} is creating a new expense.", userId);
        var expense = _mapper.Map<Expense>(dto);
        expense.UserId = userId.Value;

        var newId = await _expenseRepository.AddExpenseAsync(expense);
        if (newId == 0)
        {
            _logger.LogError("Could not create expense for user {UserId}.", userId);
            return BadRequest("Could not create expense.");
        }

        var newExpense = await _expenseRepository.GetExpenseByIdAsync(expense.UserId, newId);
        if (newExpense == null)
        {
            _logger.LogError("Expense created, but not found on fetch for user {UserId} and expenseId {ExpenseId}.", userId, newId);
            return NotFound("Expense created, but not found on fetch.");
        }

        var readDto = _mapper.Map<ReadExpenseDto>(newExpense);
        _logger.LogInformation("Successfully created expense {ExpenseId} for user {UserId}.", newId, userId);
        return CreatedAtAction(nameof(GetExpenseById), new { expenseId = newId }, readDto);
    }

    [HttpPut("{expenseId}")]
    public async Task<ActionResult> UpdateExpense(int expenseId, [FromBody] ExpenseForUpdateDto dto)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt to update expense {ExpenseId}.", expenseId);
            return Unauthorized();
        }

        _logger.LogInformation("User {UserId} is updating expense {ExpenseId}.", userId, expenseId);
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId.Value, expenseId);
        if (expense == null)
        {
            _logger.LogWarning("Expense {ExpenseId} not found for user {UserId} during update.", expenseId, userId);
            return NotFound();
        }

        _mapper.Map(dto, expense);
        await _expenseRepository.UpdateExpenseAsync(expense);
        _logger.LogInformation("Successfully updated expense {ExpenseId} for user {UserId}.", expenseId, userId);
        return NoContent();
    }

    [HttpDelete("{expenseId}")]
    public async Task<ActionResult> DeleteExpense(int expenseId)
    {
        var userId = await _userService.GetCurrentUserId();
        if (userId == null)
        {
            _logger.LogWarning("Unauthorized access attempt to delete expense {ExpenseId}.", expenseId);
            return Unauthorized();
        }

        _logger.LogInformation("User {UserId} is deleting expense {ExpenseId}.", userId, expenseId);
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId.Value, expenseId);
        if (expense == null)
        {
            _logger.LogWarning("Expense {ExpenseId} not found for user {UserId} during delete.", expenseId, userId);
            return NotFound();
        }

        await _expenseRepository.DeleteExpenseAsync(userId.Value, expenseId);
        _logger.LogInformation("Successfully deleted expense {ExpenseId} for user {UserId}.", expenseId, userId);
        return NoContent();
    }
}
