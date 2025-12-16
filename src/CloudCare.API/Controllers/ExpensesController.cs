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
        int userId = 1; // TODO (future): Extract from JWT
        var expenses = await _expenseRepository.GetExpensesAsync(userId);

        var expenseDtos = _mapper.Map<IEnumerable<Expense>, IEnumerable<ReadExpenseDto>>(expenses);

        // TODO (new): Replace hardcoded UserId with token-derived UserId in future
        // TODO (new): Validate that the user requesting this matches the token UserId (ownership check)

        return Ok(expenseDtos);
    }

    [HttpGet("{expenseId}")]
    public async Task<ActionResult<ReadExpenseDto>> GetExpenseById(int expenseId)
    {
        int userId = 1;
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId, expenseId);
        if (expense == null)
            return NotFound();

        var readDto = _mapper.Map<ReadExpenseDto>(expense);
        return Ok(readDto);
    }

    [HttpPost]
    public async Task<ActionResult<ReadExpenseDto>> CreateExpense([FromBody] ExpenseForCreationDto dto)
    {
        var expense = _mapper.Map<Expense>(dto);

        // Set UserId (replace with token logic later)
        expense.UserId = 1;

        // Save and get new ID
        var newId = await _expenseRepository.AddExpenseAsync(expense);

        if (newId == 0)
            return BadRequest("Could not create expense.");

        // Fetch with navigation properties
        var newExpense = await _expenseRepository.GetExpenseByIdAsync(expense.UserId, newId);
        // Log or inspect:
        Console.WriteLine($"Category: {newExpense?.Category?.Name}");
        Console.WriteLine($"Vendor: {newExpense?.Vendor?.Name}");
        Console.WriteLine($"PaymentMethod: {newExpense?.PaymentMethod?.Name}");

        if (newExpense == null)
            return NotFound("Expense created, but not found on fetch.");

        // Map to DTO
        var readDto = _mapper.Map<ReadExpenseDto>(newExpense);

        // Return Created (201) with location header
        return CreatedAtAction(nameof(GetExpenseById), new { expenseId = newId }, readDto);
    }

    [HttpPut("{expenseId}")]
    public async Task<ActionResult> UpdateExpense(int expenseId, [FromBody] ExpenseForUpdateDto dto)
    {
        int userId = 1;

        // Check if the user owns this expense
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId, expenseId);
        if (expense == null)
        {
            return NotFound();
        }

        // Map changes from DTO to the existing expense
        _mapper.Map(dto, expense);

        // Call the update method
        await _expenseRepository.UpdateExpenseAsync(expense);

        // 204 is used for delete and update
        return NoContent();
    }

    [HttpDelete("{expenseId}")]
    public async Task<ActionResult> DeleteExpense(int expenseId)
    {
        int userId = 1;

        var expense = await _expenseRepository.GetExpenseByIdAsync(userId, expenseId);
        if (expense == null)
        {
            return NotFound();
        }

        await _expenseRepository.DeleteExpenseAsync(userId, expenseId);

        return NoContent(); // 204
    }
}
