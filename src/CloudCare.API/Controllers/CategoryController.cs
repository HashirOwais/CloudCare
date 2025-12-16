using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryRepository categoryRepository, ILogger <CategoryController> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{categoryName}")]
    public async Task<ActionResult<Category>> GetCategoryByName(string categoryName)
    {
        var category = await _categoryRepository.GetByNameAsync(categoryName);

        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }
}