using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/vendors")]
public class VendorController : ControllerBase
{
    private readonly IVendorRepository _vendorRepository;

    public VendorController(
        IVendorRepository vendorRepository
        )
    {
        _vendorRepository = vendorRepository;
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