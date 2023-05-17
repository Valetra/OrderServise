using apiForRadBot.Data.Models;

namespace apiForRadBot.Core.Services.Interfaces;

public interface IBotService
{
    //Supply processing
    Task<IEnumerable<Supply>> GetAllSupplies();
    Task<Supply?> GetSupply(Guid id);
    Task<Supply> AddSupply(Supply supply);
    Task<Supply> UpdateSupply(Supply supply);
    Task DeleteSupply(Guid id);

    //Order processing
    Task<IEnumerable<Order>> GetAllOrders();
    Task<Order?> GetOrder(Guid id);
    Task<Order> AddOrder(Order order);
    Task<Order> UpdateOrder(Order order);
    Task DeleteOrder(Guid id);
    Task<Order> ChangeOrderStatus(Order order, string status);
    Task<Order> OrderGotPayed(Order order);
}
