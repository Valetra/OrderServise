namespace Contracts;

public interface ISupply
{
    Guid Id { get; init; }
    string Name { get; init; }
    int Count { get; init; }
    int Price { get; init; }
    TimeSpan CookingTime { get; init; }
    Guid CategoryId { get; init; }
}