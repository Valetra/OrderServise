using RESTful_API.Data.Models;

namespace RESTful_API.Data.Repositories.Interfaces;

public interface IOrderSupplyRepository
{
    Task<OrderSupply> Create(Guid orderId, Guid supplyId);
    Task<List<OrderSupply>> GetOrderSupplies(Guid orderId);
}
