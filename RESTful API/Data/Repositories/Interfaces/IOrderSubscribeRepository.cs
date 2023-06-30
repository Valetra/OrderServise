using RESTful_API.Data.Models;

namespace RESTful_API.Data.Repositories.Interfaces;

public interface IOrderSubscribeRepository
{
    Task<List<OrderSubscribe>> GetAll();
    Task<OrderSubscribe?> Get(Guid id);
    Task<OrderSubscribe> Create(OrderSubscribe orderSubscribe);
    Task<OrderSubscribe> Update(OrderSubscribe orderSubscribe);
    Task Delete(Guid id);
}
