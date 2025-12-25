using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/vendors")]
public class VendorController : ControllerBase
{
    private readonly IVendorRepository _vendorRepository;
    private readonly ILogger<VendorController> _logger;

    public VendorController(
        IVendorRepository vendorRepository,
        ILogger <VendorController> logger
        )
    {
        _vendorRepository = vendorRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vendor>>> getAllVendors()
    {
        _logger.LogInformation("getAllVendors called");
        var vendors = await _vendorRepository.GetAllAsync();
        _logger.LogInformation("getAllVendors finished, returning {count} vendors", vendors.Count());
        return Ok(vendors);

    }


    [HttpGet("{vendorName}")]
    public async Task<ActionResult<Vendor>> getVendorByName(string vendorName)
    {
        _logger.LogInformation("getVendorByName called with vendorName: {vendorName}", vendorName);
        var vendor = await _vendorRepository.GetByVendorNameAsync(vendorName);

        if (vendor == null)
        {
            _logger.LogWarning("Vendor with name {vendorName} not found", vendorName);
            return NotFound();
        }

        _logger.LogInformation("getVendorByName finished, returning vendor with id: {vendorId}", vendor.Id);
        return Ok(vendor);

    }
}