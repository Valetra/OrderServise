using apiForRadBot.Core.Services.Interfaces;
using apiForRadBot.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data;

[Route("api/[controller]")]
[ApiController]
public class BotController : ControllerBase
{
    private readonly IBotService _botService;
    public BotController(IBotService botService)
    {
        _botService = botService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Supply>>> GetAll()
    {
        return Ok(await _botService.GetAll());
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Supply>> GetSupply(Guid id)
    {
        var supply = await _botService.Get(id);
        return (supply == null) ? Ok(supply) : NotFound();
    }


    [HttpPost]
    public async Task<ActionResult<Supply>> PostUser(Supply supply)
    {
        Supply newSupply;
        newSupply = await _botService.Add(supply);

        return CreatedAtAction(nameof(GetSupply), new { id = newSupply.Id }, newSupply);
    }

    //FIX this method
    [HttpPut("{id}")]
    public async Task<ActionResult<Supply>> PutSupply(Guid id, Supply supply)
    {
        try
        {
            var supplyToUpdate = await _botService.Get(id);

            if (supplyToUpdate == null)
                return NotFound($"Supply with id = {id} not found");

            return await _botService.Update(supply);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Error updating data");
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSupply(Guid id)
    {
        await _botService.Delete(id);
        return NoContent();
    }
}
