using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly ILogger<PaymentMethodController> _logger;

    public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository, ILogger<PaymentMethodController> logger)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetAllPaymentMethods()
    {
        _logger.LogInformation("Getting all payment methods");
        var methods = await _paymentMethodRepository.GetAllAsync();
        _logger.LogInformation("Successfully retrieved all payment methods");
        return Ok(methods);
    }

    [HttpGet("{paymentMethodName}")]
    public async Task<ActionResult<PaymentMethod>> GetPaymentMethodByName(string paymentMethodName)
    {
        _logger.LogInformation("Getting payment method by name {PaymentMethodName}", paymentMethodName);
        var paymentMethod = await _paymentMethodRepository.GetByNameAsync(paymentMethodName);

        if (paymentMethod == null)
        {
            _logger.LogWarning("Payment method with name {PaymentMethodName} not found", paymentMethodName);
            return NotFound();
        }

        _logger.LogInformation("Successfully retrieved payment method by name {PaymentMethodName}", paymentMethodName);
        return Ok(paymentMethod);
    }
}