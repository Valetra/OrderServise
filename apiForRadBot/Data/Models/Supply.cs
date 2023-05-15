using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class Supply
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Price { get; set; }
    public TimeSpan? CookingTime { get; set; }
}
