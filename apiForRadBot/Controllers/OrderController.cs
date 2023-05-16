using apiForRadBot.Core.Services.Interfaces;
using apiForRadBot.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apiForRadBot.Controllers;

//Установить контроллер в рабочее состояние
[Route("[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IBotService _botService;
    public OrderController(IBotService botService)
    {
        _botService = botService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        return Ok(await _botService.GetAll());
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(Guid id)
    {
        var supply = await _botService.Get(id);
        return (supply != null) ? Ok(supply) : NotFound($"Supply with id = {id}, was not found.");
    }


    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(Supply supply)
    {
        Supply newSupply;
        newSupply = await _botService.Add(supply);

        return CreatedAtAction(nameof(GetOrder), new { id = newSupply.Id }, newSupply);
    }

    [HttpPut]
    public async Task<ActionResult<Order>> PutOrder(Order order)
    {
        //Supply existsSupply = await _botService.Get(order.Id);

        //if (existsSupply == null)
        //    return NotFound($"Supply with id = {order.Id}, was not found.");

        ////existsSupply.Name = order.Name;
        ////existsSupply.Price = order.Price;
        ////existsSupply.CookingTime = order.CookingTime;

        //return await _botService.Update(order);
        return order;
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _botService.Delete(id);
        return NoContent();
    }
}
