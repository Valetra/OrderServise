namespace Contracts;

public interface IOrderSubscribe
{
    Guid OrderId { get; init; }
    long CallbackData { get; init; }
}
