using AutoMapper;
using CloudCare.Shared.DTOs.ExpenseTracker;
using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using CloudCare.Business.Services;
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
    private readonly IExpenseService _expenseService;
    private readonly ILogger<ExpensesController> _logger;

    public ExpensesController(
        IExpenseRepository expenseRepository,
        IMapper mapper,
        IExpenseService expenseService,
        ILogger <ExpensesController> logger
    )
    {
        _expenseRepository = expenseRepository;
        _mapper = mapper;
        _expenseService = expenseService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReadExpenseDto>>> GetAllExpenses()
    {
        _logger.LogInformation("GetAllExpenses called");
        int userId = 1; // TODO (future): Extract from JWT
        var expenses = await _expenseRepository.GetExpensesAsync(userId);

        var expenseDtos = _mapper.Map<IEnumerable<Expense>, IEnumerable<ReadExpenseDto>>(expenses);

        // TODO (new): Replace hardcoded UserId with token-derived UserId in future
        // TODO (new): Validate that the user requesting this matches the token UserId (ownership check)
        _logger.LogInformation("GetAllExpenses finished, returning {count} expenses", expenseDtos.Count());
        return Ok(expenseDtos);
    }

    [HttpGet("{expenseId}")]
    public async Task<ActionResult<ReadExpenseDto>> GetExpenseById(int expenseId)
    {
        _logger.LogInformation("GetExpenseById called with expenseId: {expenseId}", expenseId);
        int userId = 1;
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId, expenseId);
        if (expense == null)
        {
            _logger.LogWarning("Expense with id {expenseId} not found", expenseId);
            return NotFound();
        }

        var readDto = _mapper.Map<ReadExpenseDto>(expense);
        _logger.LogInformation("GetExpenseById finished, returning expense with id: {expenseId}", expenseId);
        return Ok(readDto);
    }

    [HttpPost]
    public async Task<ActionResult<ReadExpenseDto>> CreateExpense([FromBody] ExpenseForCreationDto dto)
    {
        _logger.LogInformation("CreateExpense called");
        var expense = _mapper.Map<Expense>(dto);

        // Set UserId (replace with token logic later)
        expense.UserId = 1;

        // Save and get new ID
        var newId = await _expenseRepository.AddExpenseAsync(expense);

        if (newId == 0)
        {
            _logger.LogError("Could not create expense.");
            return BadRequest("Could not create expense.");
        }

        // Fetch with navigation properties
        var newExpense = await _expenseRepository.GetExpenseByIdAsync(expense.UserId, newId);
        // Log or inspect:
        Console.WriteLine($"Category: {newExpense?.Category?.Name}");
        Console.WriteLine($"Vendor: {newExpense?.Vendor?.Name}");
        Console.WriteLine($"PaymentMethod: {newExpense?.PaymentMethod?.Name}");

        if (newExpense == null)
        {
            _logger.LogError("Expense created, but not found on fetch.");
            return NotFound("Expense created, but not found on fetch.");
        }

        // Map to DTO
        var readDto = _mapper.Map<ReadExpenseDto>(newExpense);

        // Return Created (201) with location header
        _logger.LogInformation("CreateExpense finished, returning new expense with id: {newId}", newId);
        return CreatedAtAction(nameof(GetExpenseById), new { expenseId = newId }, readDto);
    }

    [HttpPut("{expenseId}")]
    public async Task<ActionResult> UpdateExpense(int expenseId, [FromBody] ExpenseForUpdateDto dto)
    {
        _logger.LogInformation("UpdateExpense called for expenseId: {expenseId}", expenseId);
        int userId = 1;

        // Check if the user owns this expense
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId, expenseId);
        if (expense == null)
        {
            _logger.LogWarning("Expense with id {expenseId} not found for user {userId}", expenseId, userId);
            return NotFound();
        }

        // Map changes from DTO to the existing expense
        _mapper.Map(dto, expense);

        // Call the update method
        await _expenseRepository.UpdateExpenseAsync(expense);

        // 204 is used for delete and update
        _logger.LogInformation("UpdateExpense finished for expenseId: {expenseId}", expenseId);
        return NoContent();
    }

    [HttpDelete("{expenseId}")]
    public async Task<ActionResult> DeleteExpense(int expenseId)
    {
        _logger.LogInformation("DeleteExpense called for expenseId: {expenseId}", expenseId);
        int userId = 1;

        var expense = await _expenseRepository.GetExpenseByIdAsync(userId, expenseId);
        if (expense == null)
        {
            _logger.LogWarning("Expense with id {expenseId} not found for user {userId}", expenseId, userId);
            return NotFound();
        }

        await _expenseRepository.DeleteExpenseAsync(userId, expenseId);

        _logger.LogInformation("DeleteExpense finished for expenseId: {expenseId}", expenseId);
        return NoContent(); // 204
    }
}
