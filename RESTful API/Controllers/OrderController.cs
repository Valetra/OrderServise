using RESTful_API.Core.Services.Interfaces;
using RESTful_API.Data.Models;
using RESTful_API.Data.RequestObject;
using Microsoft.AspNetCore.Mvc;
using RESTful_API.Data.ResponseObject;

namespace RESTful_API.Controllers;

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

    [HttpGet("getOrderWithSupplies/{id}")]
    public async Task<ActionResult<ResponseOrderObject>> GetOrderWithSupplies(Guid id)
    {
        var order = await _botService.GetOrder(id);

        if (order is null)
        {
            return NotFound($"Order with id = {id}, was not found.");
        }

        var responseOrderObject = await _botService.GetOrderSupplies(order);

        return Ok(responseOrderObject);
    }

    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder(PostOrderObject order)
    {
        Order orderEntity = new();

        orderEntity = await _botService.AddOrder(order);

        return CreatedAtAction(nameof(GetOrder), new { id = orderEntity.Id, number = orderEntity.Number }, orderEntity);
    }

    [HttpPut]
    public async Task<ActionResult<Order>> PutOrder(PutOrderObject order)
    {
        Order? existsOrder = await _botService.GetOrder(order.Id);

        if (existsOrder == null)
            return NotFound($"Order with id = {order.Id}, was not found.");

        existsOrder.Status = order.Status;
        existsOrder.Payed = order.Payed;

        return Ok(await _botService.UpdateOrder(existsOrder));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _botService.DeleteOrder(id);
        return NoContent();
    }
}
