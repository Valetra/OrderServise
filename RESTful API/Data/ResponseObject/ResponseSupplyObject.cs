namespace apiForRadBot.Data.ResponseObject;

public class ResponseSupplyObject
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public TimeSpan CookingTime { get; set; }
    public String Category { get; set; }
}
