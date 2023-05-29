using apiForRadBot.Core.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;
using apiForRadBot.Data.RequestObject;
using apiForRadBot.Core.Mapper;
using apiForRadBot.Data.Repositories.Implimentations;
using apiForRadBot.Data.ResponseObject;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace apiForRadBot.Core.Services.Implimentations;

public class BotService : IBotService
{
    private readonly ISupplyRepository _supplyRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderSupplyRepository _orderSupplyRepository;

    public BotService(ISupplyRepository supplyRepository, IOrderRepository orderRepository, IOrderSupplyRepository orderSupplyRepository)
    {
        _supplyRepository = supplyRepository;
        _orderRepository = orderRepository;
        _orderSupplyRepository = orderSupplyRepository;
    }

    //Supply processing
    public async Task<IEnumerable<Supply>> GetAllSupplies() => await _supplyRepository.GetAllSupplies();
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
    public async Task<Order> ChangeOrderStatus(Order order, string status)
    {
        order.Status = status;
        return await _orderRepository.Update(order);
    }
    public async Task<Order> OrderGotPayed(Order order)
    {
        order.Payed = true;
        return await _orderRepository.Update(order);
    }

    //OrderSupply processing
    public async Task<ResponseOrderObject> GetOrderSupplies(Order order)
    {
        List<OrderSupply> orderSupplies = await _orderSupplyRepository.GetOrderSupplies(order.Id);
        List<Supply> supplies = new();

        foreach (var item in orderSupplies)
        {
            var currentSupply = await _supplyRepository.GetSupply(item.SupplyId);
            supplies.Add(currentSupply);
        }

        return new ResponseOrderObject { Status = order.Status, Payed = order.Payed, Supplies = SupplyExtensions.ToResponseSupplies(supplies) };
    }
}
