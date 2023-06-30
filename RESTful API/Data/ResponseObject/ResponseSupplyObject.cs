using Contracts;

namespace RESTful_API.Data.ResponseObject;

public class ResponseSupplyObject : ISupply
{
    public Guid Id { get; init; }
    public string Name { get; init; } = "";
    public int Count { get; init; }
    public int Price { get; init; }
    public TimeSpan CookingTime { get; init; }
    public Guid CategoryId { get; init; }
}