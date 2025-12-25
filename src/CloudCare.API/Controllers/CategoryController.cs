using CloudCare.Shared.Models;
using CloudCare.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CloudCare.API.Controllers;

[ApiController]
[Route("/api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryRepository categoryRepository, ILogger<CategoryController> logger)
    {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        _logger.LogInformation("Getting all categories");
        var categories = await _categoryRepository.GetAllAsync();
        _logger.LogInformation("Successfully retrieved all categories");
        return Ok(categories);
    }

    [HttpGet("{categoryName}")]
    public async Task<ActionResult<Category>> GetCategoryByName(string categoryName)
    {
        _logger.LogInformation("Getting category by name {CategoryName}", categoryName);
        var category = await _categoryRepository.GetByNameAsync(categoryName);

        if (category == null)
        {
            _logger.LogWarning("Category with name {CategoryName} not found", categoryName);
            return NotFound();
        }

        _logger.LogInformation("Successfully retrieved category by name {CategoryName}", categoryName);
        return Ok(category);
    }
}