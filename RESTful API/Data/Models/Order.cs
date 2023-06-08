using apiForRadBot.Data.Models.Base;

namespace apiForRadBot.Data.Models;

public class Order : BaseModel
{
    public string Status { get; set; } = "Unconfirmed";
    public bool Payed { get; set; } = false;
    public ICollection<OrderSupply> OrderSupply { get; set; }
}