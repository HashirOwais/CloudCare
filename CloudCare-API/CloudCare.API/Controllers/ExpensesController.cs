using AutoMapper;
using CloudCare.API.DTOs;
using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IMapper _mapper;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IVendorRepository _vendorRepository;

    public ExpensesController(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository,
        IVendorRepository vendorRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IMapper mapper
    )
    {
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _mapper = mapper;
        _vendorRepository = vendorRepository;
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
        // 1. Map the DTO to an Expense entity
        var expense = _mapper.Map<Expense>(dto);

        // 2. Add user ID manually (for now)
        expense.UserId = 1;

        // TODO (new): Replace hardcoded UserId with token-derived UserId in future

        // 3. Add the expense to your repository
        await _expenseRepository.AddExpenseAsync(expense);

        // 4. Map the newly created expense to a read DTO
        var readDto = _mapper.Map<ReadExpenseDto>(expense);

        // 5. Return a 201 Created response with Location header
        return CreatedAtAction(nameof(GetExpenseById), new { expenseId = expense.Id }, readDto);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateExpense([FromBody] ExpenseForUpdateDto dto)
    {
        int userId = 1;

        // Check if the user owns this expense
        var expense = await _expenseRepository.GetExpenseByIdAsync(userId, dto.Id);
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