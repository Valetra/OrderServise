using apiForRadBot.Data.Models;

namespace apiForRadBot.Data.Repositories.Interfaces;

public interface ISupplyRepository
{
    Task<List<Supply>> GetAllSupplies();
    Task<Supply> GetSupply(Guid id);
    Task<Supply> CreateSupply(Supply supply);
    Task<Supply> UpdateSupply(Supply supply);
    Task DeleteSupply(Guid id);
}
