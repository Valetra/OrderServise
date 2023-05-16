using apiForRadBot.Data.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiForRadBot.Data.Models;

public class Order : BaseModel
{
    public List<Guid>? SuppliesId { get; set; }
    [ForeignKey("SuppliesId")]
    public virtual List<Supply>? Supplies { get; set; }
    public string? Status { get; set; }
    public bool Payed { get; set; }
}
