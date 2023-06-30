using RESTful_API.Data.Models;
using RESTful_API.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RESTful_API.Data.Repositories.Implimentations;

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

    public async Task<List<OrderSupply>> GetOrderSupplies(Guid orderId)
        => await _orderSupplies.Where(os => os.OrderId == orderId).ToListAsync();
}
