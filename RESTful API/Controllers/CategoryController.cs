using apiForRadBot.Core.Services.Interfaces;
using apiForRadBot.Data.Models;
using Microsoft.AspNetCore.Mvc;
using apiForRadBot.Data.ResponseObject;
using apiForRadBot.Data.RequestObject;
using apiForRadBot.Core.Mapper;

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
    public async Task<ActionResult<IEnumerable<ResponseCategoryObject>>> GetAll()
    {
        return Ok(await _botService.GetAllCategories());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ResponseCategoryObject>> GetCategory(Guid id)
    {
        var category = await _botService.GetCategory(id);
        return (category != null) ? Ok(category) : NotFound($"Category with id = {id}, was not found.");
    }

    [HttpPost]
    public async Task<ActionResult<ResponseCategoryObject>> PostCategory(PostCategoryObject postCategoryObject)
    {
        Category categoryEntity = await _botService.AddCategory(postCategoryObject.ToCategory());

        return CreatedAtAction(nameof(GetCategory), new { id = categoryEntity.Id }, categoryEntity);
    }

    [HttpPut]
    public async Task<ActionResult<ResponseCategoryObject>> PutCategory(PutCategoryObject putCategoryObject)
    {
        Category category = putCategoryObject.ToCategory();

        Category? existsCategory = await _botService.GetCategory(category.Id);

        if (existsCategory == null)
            return NotFound($"Category with id = {category.Id}, was not found.");

        existsCategory.Name = category.Name;

        return Ok(await _botService.UpdateCategory(existsCategory));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await _botService.DeleteCategory(id);
        return NoContent();
    }
}

