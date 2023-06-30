using RESTful_API.Data.Models;
using RESTful_API.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RESTful_API.Data.Repositories.Implimentations;

public class OrderSubscribeRepository : BaseRepository<OrderSubscribe>, IOrderSubscribeRepository
{
    private AppDbContext _context => (AppDbContext)Context;
    private DbSet<OrderSubscribe> _orderSubscribes;

    public OrderSubscribeRepository(AppDbContext context) : base(context)
    {
        _orderSubscribes = _context.OrderSubscribes;
    }

    public async Task<List<OrderSubscribe>> GetAll()
    {
        return await _orderSubscribes.ToListAsync();
    }

    public async Task<OrderSubscribe> Get(Guid id)
    {
        return await _orderSubscribes.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<OrderSubscribe> Create(OrderSubscribe orderSubscribe)
    {
        await _orderSubscribes.AddAsync(orderSubscribe);
        await _context.SaveChangesAsync();
        return orderSubscribe;
    }

    public async Task<OrderSubscribe> Update(OrderSubscribe orderSubscribe)
    {
        _context.Update(orderSubscribe);
        await _context.SaveChangesAsync();
        return orderSubscribe;
    }

    public async Task Delete(Guid id)
    {
        var toDelete = await _orderSubscribes.FirstOrDefaultAsync(m => m.Id == id);
        _orderSubscribes.Remove(toDelete);
        await _context.SaveChangesAsync();
    }
}
