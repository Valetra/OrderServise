using apiForRadBot.Core.Services.Interfaces;
using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;
using apiForRadBot.Data.RequestObject;
using apiForRadBot.Core.Mapper;
using apiForRadBot.Data.ResponseObject;

namespace apiForRadBot.Core.Services.Implimentations;

public class BotService : IBotService
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderSupplyRepository _orderSupplyRepository;
    private readonly ICategoryReposetory _categoryRepository;

    public BotService
    (
        ISupplyRepository supplyRepository,
        IOrderRepository orderRepository,
        IOrderSupplyRepository orderSupplyRepository,
        ICategoryReposetory categoryRepository
    )
    {
        _supplyRepository = supplyRepository;
        _orderRepository = orderRepository;
        _orderSupplyRepository = orderSupplyRepository;
        _categoryRepository = categoryRepository;
    }

    //Supply processing
    public async Task<List<ResponseSupplyObject>> GetAllSupplies()
    {
        return SupplyExtensions.ToResponseSupplies(await _supplyRepository.GetAllSupplies(), await _categoryRepository.GetAll());
    }
    public async Task<Supply?> GetSupply(Guid id) => await _supplyRepository.GetSupply(id);
    public async Task<Supply> AddSupply(Supply supply) => await _supplyRepository.CreateSupply(supply);
    public async Task<Supply> UpdateSupply(Supply supply) => await _supplyRepository.UpdateSupply(supply);
    public async Task DeleteSupply(Guid id) => await _supplyRepository.DeleteSupply(id);

    //Order processing
    public async Task<IEnumerable<Order>> GetAllOrders() => await _orderRepository.GetAll();
    public async Task<Order?> GetOrder(Guid id) => await _orderRepository.Get(id);
    public async Task<Order> AddOrder(PostOrderObject order)
    {
        Order newOrder = new();
        Order orderEntity = await _orderRepository.Create(newOrder);

        foreach (var supplyId in order.SuppliesId)
        {
            await _orderSupplyRepository.Create(orderEntity.Id, supplyId);
        }

        return orderEntity;
    }
    public async Task<Order> UpdateOrder(Order order) => await _orderRepository.Update(order);
    public async Task DeleteOrder(Guid id) => await _orderRepository.Delete(id);

    //OrderSupply processing
    public async Task<ResponseOrderObject> GetOrderSupplies(Order order)
    {
        List<OrderSupply> orderSupplies = await _orderSupplyRepository.GetOrderSupplies(order.Id);
        List<Supply> supplies = new();
        List<Category> categories = await _categoryRepository.GetAll();

        foreach (var item in orderSupplies)
        {
            var currentSupply = await _supplyRepository.GetSupply(item.SupplyId);
            if (currentSupply is not null)
            {
                supplies.Add(currentSupply);
            }
        }

        return new ResponseOrderObject
        {
            Status = order.Status,
            Payed = order.Payed,
            Supplies = SupplyExtensions.ToResponseSupplies(supplies, categories)
        };
    }

    //Category processing
    public async Task<IEnumerable<Category>> GetAllCategories() => await _categoryRepository.GetAll();
    public async Task<Category?> GetCategory(Guid id) => await _categoryRepository.Get(id);
    public async Task<Category> AddCategory(Category category)
    {
        Category newCategory = new();
        newCategory.Name = category.Name;

        Category categoryEntity = await _categoryRepository.Create(newCategory);

        return categoryEntity;
    }
    public async Task<Category> UpdateCategory(Category category) => await _categoryRepository.Update(category);
    public async Task DeleteCategory(Guid id) => await _categoryRepository.Delete(id);
}
