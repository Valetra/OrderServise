using RESTful_API.Data.Models;
using RESTful_API.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace RESTful_API.Data.Repositories.Implimentations;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    private AppDbContext _context => (AppDbContext)Context;
    private DbSet<Order> _orders;

    public OrderRepository(AppDbContext context) : base(context)
    {
        _orders = _context.Orders;
    }

    public async Task<List<Order>> GetAll()
    {
        return await _orders.ToListAsync();
    }

    public async Task<Order> Get(Guid id)
    {
        return await _orders.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Order> Create(Order order)
    {
        await _orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> Update(Order order)
    {
        _context.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task Delete(Guid id)
    {
        var toDelete = await _orders.FirstOrDefaultAsync(m => m.Id == id);
        _orders.Remove(toDelete);
        await _context.SaveChangesAsync();
    }
}
