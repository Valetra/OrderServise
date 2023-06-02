using apiForRadBot.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace apiForRadBot.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class Category : BaseModel
{
    public string Name { get; set; }
}

