using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/vendors")]
public class VendorController : ControllerBase
{
    private readonly IVendorRepository _vendorRepository;
    private readonly ILogger<VendorController> _logger;

    public VendorController(
        IVendorRepository vendorRepository,
        ILogger<VendorController> logger
        )
    {
        _vendorRepository = vendorRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vendor>>> getAllVendors()
    {
        _logger.LogInformation("Getting all vendors");
        var vendors = await _vendorRepository.GetAllAsync();
        _logger.LogInformation("Successfully retrieved all vendors");
        return Ok(vendors);

    }


    [HttpGet("{vendorName}")]
    public async Task<ActionResult<Vendor>> getVendorByName(string vendorName)
    {
        _logger.LogInformation("Getting vendor by name {VendorName}", vendorName);
        var vendor = await _vendorRepository.GetByVendorNameAsync(vendorName);

        if (vendor == null)
        {
            _logger.LogWarning("Vendor with name {VendorName} not found", vendorName);
            return NotFound();
        }
        
        _logger.LogInformation("Successfully retrieved vendor by name {VendorName}", vendorName);
        return Ok(vendor);

    }
}