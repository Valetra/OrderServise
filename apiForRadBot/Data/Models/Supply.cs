using apiForRadBot.Data.Models.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace apiForRadBot.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class Supply : BaseModel
{
    public string? Name { get; set; }
    public int Price { get; set; }
    public TimeSpan CookingTime { get; set; }
    public ICollection<OrderSupply>? OrderSupply { get; set; }
}