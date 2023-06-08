using apiForRadBot.Core.Services.Interfaces;
using apiForRadBot.Data.Models;
using apiForRadBot.Data.ResponseObject;
using Microsoft.AspNetCore.Mvc;

namespace apiForRadBot.Data;

[Route("[controller]")]
[ApiController]
public class SupplyController : ControllerBase
{
    private readonly IBotService _botService;
    public SupplyController(IBotService botService)
    {
        _botService = botService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ResponseSupplyObject>>> GetAll()
    {
        return Ok(await _botService.GetAllSupplies());
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Supply>> GetSupply(Guid id)
    {
        var supply = await _botService.GetSupply(id);
        return (supply != null) ? Ok(supply) : NotFound($"Supply with id = {id}, was not found.");
    }


    [HttpPost]
    public async Task<ActionResult<Supply>> PostSupply(Supply supply)
    {
        Supply newSupply;
        newSupply = await _botService.AddSupply(supply);

        return CreatedAtAction(nameof(GetSupply), new { id = newSupply.Id }, newSupply);
    }

    [HttpPut]
    public async Task<ActionResult<Supply>> PutSupply(Supply supply)
    {
        Supply? existsSupply = await _botService.GetSupply(supply.Id);

        if (existsSupply == null)
            return NotFound($"Supply with id = {supply.Id}, was not found.");

        existsSupply.Name = supply.Name;
        existsSupply.Price = supply.Price;
        existsSupply.CookingTime = supply.CookingTime;
        existsSupply.CategoryId = supply.CategoryId;

        return await _botService.UpdateSupply(existsSupply);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupply(Guid id)
    {
        await _botService.DeleteSupply(id);
        return NoContent();
    }
}
