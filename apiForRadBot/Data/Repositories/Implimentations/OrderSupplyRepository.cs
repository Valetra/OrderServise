using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace apiForRadBot.Data.Repositories.Implimentations;

public class OrderSupplyRepository : BaseRepository<OrderSupply>, IOrderSupplyRepository
{
    private AppDbContext _context => (AppDbContext)Context;
    private DbSet<OrderSupply> _orderSupplies;

    public OrderSupplyRepository(AppDbContext context) : base(context)
    {
        _orderSupplies = _context.OrderSupply;
    }

    public async Task<OrderSupply> Create(Guid orderId, Guid supplyId)
    {
        OrderSupply orderSupplyEntity = new OrderSupply(orderId, supplyId);

        await _orderSupplies.AddAsync(orderSupplyEntity);
        await _context.SaveChangesAsync();
        return orderSupplyEntity;
    }

    public async Task<List<(Guid, int)>> GetOrderSupplies(Guid orderId)
    {
        List<(Guid, int)> result = null;
        List<OrderSupply> orderSupplies = _orderSupplies.Where(os => os.OrderId == orderId).ToList();

        var q = orderSupplies.GroupBy(x => x.OrderId)
            .Select(x => new
            {
                Id = x.Key,
                Count = x.Count()
            })
            .OrderByDescending(x => x.Count).ToList();

        foreach (var item in q)
        {
            result.Add((item.Id, item.Count));
        }

        return result;
    }
}
