using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly ILogger _logger;

    public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository, ILogger logger)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetAllPaymentMethods()
    {
        var methods = await _paymentMethodRepository.GetAllAsync();
        return Ok(methods);
    }

    [HttpGet("{paymentMethodName}")]
    public async Task<ActionResult<PaymentMethod>> GetPaymentMethodByName(string paymentMethodName)
    {
        var paymentMethod = await _paymentMethodRepository.GetByNameAsync(paymentMethodName);

        if (paymentMethod == null)
        {
            return NotFound();
        }

        return Ok(paymentMethod);
    }
}