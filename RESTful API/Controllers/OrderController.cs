using RESTful_API.Core.Services.Interfaces;
using RESTful_API.Data.Models;
using RESTful_API.Data.RequestObject;
using Microsoft.AspNetCore.Mvc;
using RESTful_API.Data.ResponseObject;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace RESTful_API.Controllers;

[Route("[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private static bool IsOrderChanged = true;
    private readonly IBotService _botService;
    public OrderController(IBotService botService)
    {
        _botService = botService;
    }

    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            if (webSocket is not null)
            {
                await OrdersHook(webSocket);
            }
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task OrdersHook(WebSocket webSocket)
    {

        string jsonOrders = JsonSerializer.Serialize(await _botService.GetAllOrders());

        byte[] orderBytes = Encoding.ASCII.GetBytes(jsonOrders);

        var receiveResult = await webSocket.ReceiveAsync(
            new ArraySegment<byte>(orderBytes), CancellationToken.None);

        while (!receiveResult.CloseStatus.HasValue)
        {
            if (IsOrderChanged)
            {
                jsonOrders = JsonSerializer.Serialize(
                    await _botService.GetAllOrders(),
                    new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }
                );

                orderBytes = Encoding.ASCII.GetBytes(jsonOrders);

                await webSocket.SendAsync(
                    new ArraySegment<byte>(orderBytes, 0, orderBytes.Length),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                IsOrderChanged = false;
            }

            await Task.Delay(TimeSpan.FromSeconds(3));
        }

        await webSocket.CloseAsync(
            receiveResult.CloseStatus.Value,
            receiveResult.CloseStatusDescription,
            CancellationToken.None);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetAll()
    {
        IsOrderChanged = true;
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

        IsOrderChanged = true;

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

        IsOrderChanged = true;

        return Ok(await _botService.UpdateOrder(existsOrder));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        await _botService.DeleteOrder(id);
        IsOrderChanged = true;
        return NoContent();
    }
}
