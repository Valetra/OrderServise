using RESTful_API.Core.Services.Interfaces;
using RESTful_API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using RESTful_API.Data.ResponseObject;
using RESTful_API.Data.RequestObject;
using RESTful_API.Core.Mapper;

namespace RESTful_API.Data;

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

