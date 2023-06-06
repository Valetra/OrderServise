namespace Contracts;

public interface IOrder
{
    List<Guid>? SuppliesId { get; set; }
    List<Supply> Supplies { get; set; }
}

public class Order : IOrder
{
    public List<Guid>? SuppliesId { get; set; }
    public List<Supply> Supplies { get; set; }
}