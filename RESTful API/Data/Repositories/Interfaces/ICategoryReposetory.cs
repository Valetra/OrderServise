using apiForRadBot.Data.Models;

namespace apiForRadBot.Data.Repositories.Interfaces;

public interface ICategoryReposetory
{
    Task<List<Category>> GetAll();
    Task<Category> Get(Guid id);
    Task<Category> Create(Category category);
    Task<Category> Update(Category category);
    Task Delete(Guid id);
}
