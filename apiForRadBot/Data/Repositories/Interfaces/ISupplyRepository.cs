using apiForRadBot.Data.Models;

namespace apiForRadBot.Data.Repositories.Interfaces;

public interface ISupplyRepository
{
    Task<List<Supply>> GetAll();
    Task<Supply?> Get(Guid id);
    Task<Supply> Create(Supply supply);
    Task<Supply> Update(Supply supply);
    Task Delete(Guid id);
}
