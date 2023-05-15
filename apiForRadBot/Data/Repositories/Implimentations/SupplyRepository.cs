using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data.Repositories.Implimentations;

public class SupplyRepository : ISupplyRepository
{
    private AppDbContext _context { get; set; }
    public SupplyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Supply>> GetAll()
    {
        return await _context.Supplies.ToListAsync();
    }

    public async Task<Supply?> Get(Guid id)
    {
        return await _context.Supplies.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Supply> Create(Supply supply)
    {
        await _context.Supplies.AddAsync(supply);
        await _context.SaveChangesAsync();
        return supply;
    }

    public async Task<Supply> Update(Supply supply)
    {
        var toUpdate = await _context.Supplies.FirstOrDefaultAsync(m => m.Id == supply.Id);
        if (toUpdate != null)
        {
            toUpdate = supply;
        }

        _context.Update(toUpdate);
        await _context.SaveChangesAsync();
        return toUpdate;
    }

    public async Task Delete(Guid id)
    {
        var toDelete = await _context.Supplies.FirstOrDefaultAsync(m => m.Id == id);
        _context.Supplies.Remove(toDelete);
        await _context.SaveChangesAsync();
    }
}