using RESTful_API.Data.Models.Base;

namespace RESTful_API.Data.Models;

public class Order : BaseModel
{
    public string Status { get; set; } = "Unconfirmed";
    public bool Payed { get; set; } = false;
    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public int Number { get; set; }
    public ICollection<OrderSupply> OrderSupply { get; set; }
}