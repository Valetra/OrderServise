using apiForRadBot.Core.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using apiForRadBot.Data.Models;

namespace apiForRadBot.Core.Services.Implimentations;

public class BotService : IBotService
{
    private readonly IBotRepository _botRepository;

    public BotService(IBotRepository botRepository)
    {
        _botRepository = botRepository;
    }

    public async Task<IEnumerable<Supply>> GetAll() => await _botRepository.GetAll();
    public async Task<Supply> Get(Guid id) => await _botRepository.Get(id);

    public async Task<Supply> Add(Supply supply)
    {
        return await _botRepository.Create(supply);

    }

    public async Task<Supply> Update(Supply supply)
    {
        return await _botRepository.Update(supply);
    }

    public async Task Delete(Guid id)
    {
        return await _botRepository.Delete(id);
    }
}
