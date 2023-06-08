using Contracts;

namespace Models;

public class Supply : ISupply
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public int Count { get; init; }
    public int Price { get; init; }
    public TimeSpan CookingTime { get; init; }
    public Guid CategoryId { get; init; }
}