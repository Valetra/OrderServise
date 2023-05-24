using apiForRadBot.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiForRadBot.Data.Models;

public class Order : BaseModel
{
    public string? Status { get; set; } = "Unconfirmed";
    public bool Payed { get; set; } = false;
    public ICollection<OrderSupply> OrderSupply { get; set; }
}