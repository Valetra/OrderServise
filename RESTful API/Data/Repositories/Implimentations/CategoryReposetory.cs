using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data.Repositories.Implimentations;

public class CategoryReposetory : BaseRepository<Category>, ICategoryReposetory
{
    private AppDbContext _context => (AppDbContext)Context;
    private DbSet<Category> _categories;

    public CategoryReposetory(AppDbContext context) : base(context)
    {
        _categories = _context.Categories;
    }
    public async Task<List<Category>> GetAll()
    {
        return await _categories.ToListAsync();
    }

    public async Task<Category> Get(Guid id)
    {
        return await _categories.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Category> Create(Category category)
    {
        await _categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<Category> Update(Category category)
    {
        _context.Update(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task Delete(Guid id)
    {
        var toDelete = await _categories.FirstOrDefaultAsync(m => m.Id == id);
        _categories.Remove(toDelete);
        await _context.SaveChangesAsync();
    }
}
