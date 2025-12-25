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
        _logger.LogInformation("GetAllCategories called");
        var categories = await _categoryRepository.GetAllAsync();
        _logger.LogInformation("GetAllCategories finished, returning {count} categories", categories.Count());
        return Ok(categories);
    }

    [HttpGet("{categoryName}")]
    public async Task<ActionResult<Category>> GetCategoryByName(string categoryName)
    {
        _logger.LogInformation("GetCategoryByName called with categoryName: {categoryName}", categoryName);
        var category = await _categoryRepository.GetByNameAsync(categoryName);

        if (category == null)
        {
            _logger.LogWarning("Category with name {categoryName} not found", categoryName);
            return NotFound();
        }

        _logger.LogInformation("GetCategoryByName finished, returning category with id: {categoryId}", category.Id);
        return Ok(category);
    }
}