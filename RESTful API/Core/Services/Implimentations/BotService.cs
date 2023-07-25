using RESTful_API.Core.Services.Interfaces;
using RESTful_API.Data.Models;
using RESTful_API.Data.Repositories.Interfaces;
using RESTful_API.Data.RequestObject;
using RESTful_API.Core.Mapper;
using RESTful_API.Data.ResponseObject;
using System.Data;
using RESTful_API.MessengerLogic;
using System.Net.WebSockets;

namespace RESTful_API.Core.Services.Implimentations;

public class BotService : IBotService
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderSupplyRepository _orderSupplyRepository;
    private readonly ICategoryReposetory _categoryRepository;
    private readonly IOrderSubscribeRepository _orderSubscribeRepository;

    private List<OrderStatus> _statuses = new();

    public BotService
    (
        ISupplyRepository supplyRepository,
        IOrderRepository orderRepository,
        IOrderSupplyRepository orderSupplyRepository,
        ICategoryReposetory categoryRepository,
        IOrderSubscribeRepository orderSubscribeRepository
    )
    {
        _supplyRepository = supplyRepository;
        _orderRepository = orderRepository;
        _orderSupplyRepository = orderSupplyRepository;
        _categoryRepository = categoryRepository;
        _orderSubscribeRepository = orderSubscribeRepository;

        _statuses.Add(new("Unconfirmed", "Не подтверждён"));
        _statuses.Add(new("Confirmed", "Подтверждён"));
        _statuses.Add(new("In progress", "В процессе"));
        _statuses.Add(new("Done", "Готов"));
        _statuses.Add(new("Cancelled", "Отменён"));
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
        IEnumerable<Order> allOrders = await _orderRepository.GetAll();

        DateTime today = DateTime.Today;

        int todayOrdersCount = allOrders.Where(o => o.CreateDateTime.ToLocalTime() >= today && o.CreateDateTime.ToLocalTime() <= today.AddDays(1))
            .Count();

        Order newOrder = new();

        var newOrderTime = newOrder.CreateDateTime.ToLocalTime();

        newOrder.Number = todayOrdersCount + 1;

        Order orderEntity = await _orderRepository.Create(newOrder);

        foreach (var supplyId in order.SuppliesId)
        {
            await _orderSupplyRepository.Create(orderEntity.Id, supplyId);
        }

        return orderEntity;
    }
    public async Task<Order> UpdateOrder(Order order)
    {
        List<OrderSubscribe> orderSubscribes = await _orderSubscribeRepository.GetAll();
        var chaId = orderSubscribes.Where(o => o.OrderId == order.Id).Select(o => o.CallbackData).First();

        string orderStatus = _statuses.Where(o => o.Value == order.Status).Select(o => o.Label).First();

        Message message = new() { Chat_id = chaId, Text = $"Номер вашего заказа #{order.Number}\nСтатус заказа: {orderStatus}" };

        var response = await MessageManager.SendMessageToChat(message);

        return await _orderRepository.Update(order);
    }
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
            CreateDateTime = order.CreateDateTime,
            Number = order.Number,
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

    //OrderSubscribe processing
    public async Task<IEnumerable<OrderSubscribe>> GetAllSubscribes() => await _orderSubscribeRepository.GetAll();
    public async Task<OrderSubscribe?> GetOrderSubscribe(Guid id) => await _orderSubscribeRepository.Get(id);
    public async Task<OrderSubscribe> AddOrderSubscribe(OrderSubscribe orderSubscribe)
    {
        OrderSubscribe newOrderSubscribe = new()
        {
            OrderId = orderSubscribe.OrderId,
            CallbackData = orderSubscribe.CallbackData
        };

        OrderSubscribe orderSubscribeEntity = await _orderSubscribeRepository.Create(newOrderSubscribe);

        return orderSubscribeEntity;
    }
    public async Task<OrderSubscribe> UpdateOrderSubscribe(OrderSubscribe orderSubscribe) => await _orderSubscribeRepository.Update(orderSubscribe);
    public async Task DeleteOrderSubscribe(Guid id) => await _orderSubscribeRepository.Delete(id);
}
