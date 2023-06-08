namespace Contracts;

public interface IOrder
{
    List<Guid> SuppliesId { get; }
}