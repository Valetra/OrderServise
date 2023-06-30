using RESTful_API.Data.Models;
using RESTful_API.Data.RequestObject;
using RESTful_API.Data.ResponseObject;

namespace RESTful_API.Core.Services.Interfaces;

public interface IBotService
{
    //Supply processing
    Task<List<ResponseSupplyObject>> GetAllSupplies();
    Task<Supply?> GetSupply(Guid id);
    Task<Supply> AddSupply(Supply supply);
    Task<Supply> UpdateSupply(Supply supply);
    Task DeleteSupply(Guid id);

    //Order processing
    Task<IEnumerable<Order>> GetAllOrders();
    Task<Order?> GetOrder(Guid id);
    Task<Order> AddOrder(PostOrderObject order);
    Task<Order> UpdateOrder(Order order);
    Task DeleteOrder(Guid id);

    //OrderSupply processing
    Task<ResponseOrderObject> GetOrderSupplies(Order order);

    //Category processing
    Task<IEnumerable<Category>> GetAllCategories();
    Task<Category?> GetCategory(Guid id);
    Task<Category> AddCategory(Category category);
    Task<Category> UpdateCategory(Category category);
    Task DeleteCategory(Guid id);
}
