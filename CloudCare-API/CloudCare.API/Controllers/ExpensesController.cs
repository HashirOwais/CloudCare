using System.Security.AccessControl;
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

   
    [HttpGet("{UserId}")]
    public async Task<ActionResult<IEnumerable<ReadExpenseDto>>> GetAllExpenses(int UserId)
    {
        var expenses = await _expenseRepository.GetExpensesAsync(UserId);

        
        var expenseDtos = _mapper.Map<IEnumerable<Expense>, IEnumerable<ReadExpenseDto>>(expenses);        
       
        //TODO: Work on the mapper such that it maps the 3 entites into the DTO IT is currently sending out nulls
        
        
        return Ok(expenseDtos);
    }

    [HttpPost]
    public async Task<ActionResult<ReadExpenseDto>> CreateExpense([FromBody] ExpenseForCreationDto dto)
    {
        // 1. Map the DTO to an Expense entity
        var expense = _mapper.Map<Expense>(dto);

        // 2. Add user ID manually (for now)
        expense.UserId = 1;

        // 3. Add the expense to your repository
        await _expenseRepository.AddExpenseAsync(expense);

        // 4. Map the newly created expense to a read DTO
        var readDto = _mapper.Map<ReadExpenseDto>(expense);

        // 5. Return a 201 Created response
        return CreatedAtAction(nameof(GetExpenseById), new { id = expense.Id }, readDto);
    }
    
}