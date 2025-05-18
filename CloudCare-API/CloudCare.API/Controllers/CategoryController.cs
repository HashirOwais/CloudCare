using CloudCare.API.Models;
using CloudCare.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }
}