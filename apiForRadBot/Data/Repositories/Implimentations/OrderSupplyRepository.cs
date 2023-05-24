using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;
using apiForRadBot.Data.RequestObject;
using Microsoft.EntityFrameworkCore;

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
}
