using apiForRadBot.Core.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using apiForRadBot.Data.Models;
using apiForRadBot.Data.Repositories.Interfaces;

namespace apiForRadBot.Core.Services.Implimentations;

public class BotService : IBotService
{
    private readonly ISupplyRepository _supplyRepository;

    public BotService(ISupplyRepository supplyRepository)
    {
        _supplyRepository = supplyRepository;
    }

    public async Task<IEnumerable<Supply>> GetAll() => await _supplyRepository.GetAll();
    public async Task<Supply> Get(Guid id) => await _supplyRepository.Get(id);

    public async Task<Supply> Add(Supply supply)
    {
        return await _supplyRepository.Create(supply);

    }

    public async Task<Supply> Update(Supply supply)
    {
        return await _supplyRepository.Update(supply);
    }

    public async Task Delete(Guid id)
    {
        await _supplyRepository.Delete(id);
    }
}
