using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/paymentmethods")]
public class PaymentMethodController : ControllerBase
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
    {
        _paymentMethodRepository = paymentMethodRepository;
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