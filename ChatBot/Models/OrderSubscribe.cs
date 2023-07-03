using Contracts;

namespace Models;
public class OrderSubscribe : IOrderSubscribe
{
    public Guid OrderId { get; init; }
    public long CallbackData { get; init; }
    public OrderSubscribe(Guid orderId, long callbackData)
    {
        OrderId = orderId;
        CallbackData = callbackData;
    }
}