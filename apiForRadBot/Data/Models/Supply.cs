using apiForRadBot.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class Supply : BaseModel
{
    public string? Name { get; set; }
    public int Price { get; set; }
    public TimeSpan? CookingTime { get; set; }
    public List<Order>? Orders { get; set; } = new();
}