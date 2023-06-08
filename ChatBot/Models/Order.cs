using Contracts;

namespace Models;
public class Order : IOrder
{
    public List<Guid> SuppliesId { get; } = new();
}