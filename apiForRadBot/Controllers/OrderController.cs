using apiForRadBot.Core.Services.Interfaces;
using apiForRadBot.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apiForRadBot.Controllers;

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
        return Ok(await _botService.GetAllOrders());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(Guid id)
    {
        var order = await _botService.GetOrder(id);
        return (order != null) ? Ok(order) : NotFound($"Order with id = {id}, was not found.");
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(Order order)
    {
        Order newOrder;
        newOrder = await _botService.AddOrder(order);

        return CreatedAtAction(nameof(GetOrder), new { id = newOrder.Id }, newOrder);
    }

    [HttpPut]
    public async Task<ActionResult<Order>> PutOrder(Order order)
    {
        Order? existsOrder = await _botService.GetOrder(order.Id);

        if (existsOrder == null)
            return NotFound($"Order with id = {order.Id}, was not found.");

        //existsOrder.Supplies = order.Supplies;
        existsOrder.Status = order.Status;
        existsOrder.Payed = order.Payed;

        return await _botService.UpdateOrder(order);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _botService.DeleteOrder(id);
        return NoContent();
    }
}
