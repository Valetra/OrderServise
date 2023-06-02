using apiForRadBot.Core.Services.Interfaces;
using apiForRadBot.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiForRadBot.Data;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IBotService _botService;
    public CategoryController(IBotService botService)
    {
        _botService = botService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetAll()
    {
        return Ok(await _botService.GetAllCategories());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(Guid id)
    {
        var category = await _botService.GetCategory(id);
        return (category != null) ? Ok(category) : NotFound($"Category with id = {id}, was not found.");
    }

    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category Category)
    {
        Category categoryEntity = new();

        categoryEntity = await _botService.AddCategory(Category);

        return CreatedAtAction(nameof(GetCategory), new { id = categoryEntity.Id }, categoryEntity);
    }

    [HttpPut("{categoryId}")]
    public async Task<ActionResult<Category>> PutCategory(String categoryName, Guid categoryId)
    {
        Category? existsCategory = await _botService.GetCategory(categoryId);

        if (existsCategory == null)
            return NotFound($"Category with id = {categoryId}, was not found.");

        existsCategory.Name = categoryName;

        return Ok(await _botService.UpdateCategory(existsCategory));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await _botService.DeleteCategory(id);
        return NoContent();
    }
}

