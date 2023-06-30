using RESTful_API.Data.Models.Base;

namespace RESTful_API.Data.Models;

public class OrderSupply : BaseModel
{
    public new int Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public Guid SupplyId { get; set; }
    public Supply Supply { get; set; }

    public OrderSupply(Guid orderId, Guid supplyId)
    {
        OrderId = orderId;
        SupplyId = supplyId;
    }
}
