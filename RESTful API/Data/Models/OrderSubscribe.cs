using RESTful_API.Data.Models.Base;

namespace RESTful_API.Data.Models;

public class OrderSubscribe : BaseModel
{
    public Guid OrderId { get; set; }
    public long CallbackData { get; set; }
}