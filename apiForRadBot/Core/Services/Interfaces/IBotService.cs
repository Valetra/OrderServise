using apiForRadBot.Data.Models;

namespace apiForRadBot.Core.Services.Interfaces;

public interface IBotService
{
    Task<IEnumerable<Supply>> GetAll();
    Task<Supply?> Get(Guid id);
    Task<Supply> Add(Supply supply);
    Task<Supply> Update(Supply supply);
    Task Delete(Guid id);
}
