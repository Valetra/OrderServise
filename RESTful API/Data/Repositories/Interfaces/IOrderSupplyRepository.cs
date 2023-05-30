using apiForRadBot.Data.Models;

namespace apiForRadBot.Data.Repositories.Interfaces;

public interface IOrderSupplyRepository
{
    Task<OrderSupply> Create(Guid orderId, Guid supplyId);
    Task<List<OrderSupply>> GetOrderSupplies(Guid orderId);
}
