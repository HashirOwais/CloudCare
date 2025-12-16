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
        var vendors = await _vendorRepository.GetAllAsync();

        return Ok(vendors);

    }


    [HttpGet("{vendorName}")]
    public async Task<ActionResult<Vendor>> getVendorByName(string vendorName)
    {
        var vendor = await _vendorRepository.GetByVendorNameAsync(vendorName);

        if (vendor == null)
        {
            return NotFound();
        }

        return Ok(vendor);

    }
}