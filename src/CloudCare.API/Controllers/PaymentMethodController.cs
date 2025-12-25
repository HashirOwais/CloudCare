using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        _logger.LogInformation("GetAllPaymentMethods called");
        var methods = await _paymentMethodRepository.GetAllAsync();
        _logger.LogInformation("GetAllPaymentMethods finished, returning {count} methods", methods.Count());
        return Ok(methods);
    }

    [HttpGet("{paymentMethodName}")]
    public async Task<ActionResult<PaymentMethod>> GetPaymentMethodByName(string paymentMethodName)
    {
        _logger.LogInformation("GetPaymentMethodByName called with paymentMethodName: {paymentMethodName}", paymentMethodName);
        var paymentMethod = await _paymentMethodRepository.GetByNameAsync(paymentMethodName);

        if (paymentMethod == null)
        {
            _logger.LogWarning("PaymentMethod with name {paymentMethodName} not found", paymentMethodName);
            return NotFound();
        }

        _logger.LogInformation("GetPaymentMethodByName finished, returning paymentMethod with id: {paymentMethodId}", paymentMethod.Id);
        return Ok(paymentMethod);
    }
}