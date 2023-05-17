using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data.Repositories.Implimentations;

public class SupplyRepository : BaseRepository<Supply>, ISupplyRepository
{
    private AppDbContext _context => (AppDbContext)Context;
    private DbSet<Supply> _supplies;

    public SupplyRepository(AppDbContext context) : base(context)
    {
        _supplies = _context.Supplies;
    }

    public async Task<List<Supply>> GetAllSupplies()
    {
        return await _supplies.ToListAsync();
    }

    public async Task<Supply?> GetSupply(Guid id)
    {
        return await _supplies.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Supply> CreateSupply(Supply supply)
    {
        await _supplies.AddAsync(supply);
        await _context.SaveChangesAsync();
        return supply;
    }

    public async Task<Supply> UpdateSupply(Supply supply)
    {
        _context.Update(supply);
        await _context.SaveChangesAsync();
        return supply;
    }

    public async Task DeleteSupply(Guid id)
    {
        var toDelete = await _supplies.FirstOrDefaultAsync(m => m.Id == id);
        _supplies.Remove(toDelete);
        await _context.SaveChangesAsync();
    }
}