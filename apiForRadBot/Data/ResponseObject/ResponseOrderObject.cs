using apiForRadBot.Data.Models;

namespace apiForRadBot.Data.ResponseObject;

public class ResponseOrderObject
{
    public string Status { get; set; }
    public bool Payed { get; set; }
    public List<ResponseSupply>? Supplies { get; set; }
}

public class ResponseSupply
{
    public string? Name { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public TimeSpan? CookingTime { get; set; }
}
