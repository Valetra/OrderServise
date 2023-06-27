using apiForRadBot.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data.Models;

public class Supply : BaseModel
{
    public string Name { get; set; }
    public int Price { get; set; }
    public TimeSpan CookingTime { get; set; }
    public Guid CategoryId { get; set; }
    public ICollection<OrderSupply>? OrderSupply { get; set; }
}