using RESTful_API.Core.Services.Interfaces;
using RESTful_API.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace RESTful_API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrderSubscribeController : ControllerBase
{
    private readonly IBotService _botService;
    public OrderSubscribeController(IBotService botService)
    {
        _botService = botService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderSubscribe>>> GetAll()
    {
        return Ok(await _botService.GetAllSubscribes());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderSubscribe>> GetSubscribe(Guid id)
    {
        var orderSubscribe = await _botService.GetOrderSubscribe(id);
        return (orderSubscribe != null) ? Ok(orderSubscribe) : NotFound($"OrderSubscribe with id = {id}, was not found.");
    }

    [HttpPost]
    public async Task<ActionResult<OrderSubscribe>> PostOrderSubscribe(OrderSubscribe orderSubscribe)
    {
        OrderSubscribe orderSubscribeEntity = new();

        orderSubscribeEntity = await _botService.AddOrderSubscribe(orderSubscribe);

        return CreatedAtAction(nameof(GetSubscribe), new { id = orderSubscribeEntity.Id }, orderSubscribeEntity);
    }

    [HttpPut]
    public async Task<ActionResult<OrderSubscribe>> PutOrderSubscribe(OrderSubscribe orderSubscribe)
    {
        OrderSubscribe? existsOrderSubscribe = await _botService.GetOrderSubscribe(orderSubscribe.Id);

        if (existsOrderSubscribe == null)
            return NotFound($"OrderSubscribe with id = {orderSubscribe.Id}, was not found.");

        existsOrderSubscribe.OrderId = orderSubscribe.OrderId;
        existsOrderSubscribe.CallbackData = orderSubscribe.CallbackData;

        return Ok(await _botService.UpdateOrderSubscribe(existsOrderSubscribe));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _botService.DeleteOrderSubscribe(id);
        return NoContent();
    }
}
