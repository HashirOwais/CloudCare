using AutoMapper;
using CloudCare.Shared.DTOs.ExpenseTracker;
using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using CloudCare.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using OpenAI.Chat;
using System.Text.Json;
using System.ClientModel;

namespace CloudCare.API.Controllers;

//[Authorize]
[ApiController]
[Route("/api/expenses")]
public class ExpensesController : ControllerBase
{
    public IPaymentMethodRepository PaymentMethodRepository { get; }
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IVendorRepository _vendorRepository;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ILogger<ExpensesController> _logger;
    private readonly IConfiguration _configuration;
    private readonly DocumentIntelligenceClient _documentIntelligenceClient;
    private readonly AzureOpenAIClient _azureOpenAIClient;

    public ExpensesController(
        IExpenseRepository expenseRepository,
        ICategoryRepository categoryRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IVendorRepository vendorRepository,
        IMapper mapper,
        IUserService userService,
        ILogger<ExpensesController> logger,
        IConfiguration configuration,
        DocumentIntelligenceClient documentIntelligenceClient,
        AzureOpenAIClient azureOpenAIClient
    )
    {
        PaymentMethodRepository = paymentMethodRepository;
        _expenseRepository = expenseRepository;
        _categoryRepository = categoryRepository;
        _vendorRepository = vendorRepository;
        _mapper = mapper;
        _userService = userService;
        _logger = logger;
        _configuration = configuration;
        _documentIntelligenceClient = documentIntelligenceClient;
        _azureOpenAIClient = azureOpenAIClient;
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
    
    [HttpPost("from-photo")]
    public async Task<ActionResult<ExpenseForCreationDto>> ExtractExpenseFromReceipt([FromForm] List<IFormFile> files)
    {
        if (files == null || files.Count == 0)
            return BadRequest("No files uploaded.");

        var file = files[0];
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        BinaryData receiptData = BinaryData.FromBytes(stream.ToArray());

        Operation<AnalyzeResult> operation = await _documentIntelligenceClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-receipt", receiptData);
        AnalyzeResult receipts = operation.Value;
        var expenseDto = new ExpenseForCreationDto();

        foreach (AnalyzedDocument receipt in receipts.Documents)
        {
            if (receipt.Fields.TryGetValue("MerchantName", out DocumentField? merchantNameField))
            {
                expenseDto.Description = merchantNameField.ValueString;
            }
            if (receipt.Fields.TryGetValue("TransactionDate", out DocumentField? transactionDateField) && transactionDateField.ValueDate.HasValue)
            {
                expenseDto.Date = DateOnly.FromDateTime(transactionDateField.ValueDate.Value.DateTime);
            }
            else
            {
                expenseDto.Date = DateOnly.FromDateTime(DateTime.Now);
            }
            if (receipt.Fields.TryGetValue("Total", out DocumentField? totalField))
            {
                if (totalField.ValueCurrency != null)
                {
                    expenseDto.Amount = (decimal)totalField.ValueCurrency.Amount;
                }
                else if (totalField.ValueDouble.HasValue)
                {
                    expenseDto.Amount = (decimal)totalField.ValueDouble.Value;
                }
            }
            if (receipt.Fields.TryGetValue("Items", out DocumentField? itemsField))
            {
                var itemSummaries = new List<string>();
                foreach (DocumentField itemField in itemsField.ValueList)
                {
                    IReadOnlyDictionary<string, DocumentField> itemProperties = itemField.ValueDictionary;
                    if (itemProperties.TryGetValue("Description", out DocumentField? itemDesc) && itemDesc.ValueString != null)
                    {
                        itemSummaries.Add(itemDesc.ValueString);
                    }
                }
                expenseDto.Notes = string.Join(", ", itemSummaries);
            }
        }
        
        var categories = await _categoryRepository.GetAllAsync();
        var vendors = await _vendorRepository.GetAllAsync();
        var paymentMethods = await PaymentMethodRepository.GetAllAsync();

        var systemPrompt = $"""
You are an intelligent accounting assistant. Your task is to analyze receipt data and accurately map it to the correct Category, Vendor, and Payment Method from the provided lists.
Respond ONLY with a valid JSON object with the keys "CategoryId", "VendorId", and "PaymentMethodId".

Available Categories:
{JsonSerializer.Serialize(categories.Select(c => new { c.Id, c.Name }))}

Available Vendors:
{JsonSerializer.Serialize(vendors.Select(v => new { v.Id, v.Name }))}

Available Payment Methods:
{JsonSerializer.Serialize(paymentMethods.Select(p => new { p.Id, p.Name }))}

Based on the receipt data, determine the most appropriate IDs. If the merchant name is not in the vendors list, choose the vendor that is most similar or a generic one if applicable.
""";

        var userPrompt = $"""
Receipt Data:
- Merchant: {expenseDto.Description}
- Items: {expenseDto.Notes}

Please provide the JSON object with the correct IDs.
""";

        var chatClient = _azureOpenAIClient.GetChatClient("gpt-5-nano");

        var chatCompletionOptions = new ChatCompletionOptions()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
        };

        try
        {
            List<ChatMessage> messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };

            ClientResult<ChatCompletion> completion = await chatClient.CompleteChatAsync(messages, chatCompletionOptions);
            
            var jsonResponse = completion.Value.Content[0].Text;

            var aiResponse = JsonSerializer.Deserialize<JsonElement>(jsonResponse);

            if (aiResponse.TryGetProperty("CategoryId", out var categoryIdElement))
            {
                expenseDto.CategoryId = categoryIdElement.GetInt32();
            }
            if (aiResponse.TryGetProperty("VendorId", out var vendorIdElement))
            {
                expenseDto.VendorId = vendorIdElement.GetInt32();
            }
            if (aiResponse.TryGetProperty("PaymentMethodId", out var paymentMethodIdElement))
            {
                expenseDto.PaymentMethodId = paymentMethodIdElement.GetInt32();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Azure OpenAI service.");
            expenseDto.CategoryId = 0;
            expenseDto.VendorId = 0;
            expenseDto.PaymentMethodId = 1;
        }

        return Ok(expenseDto);
    }
}