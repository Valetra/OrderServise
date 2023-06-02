namespace apiForRadBot.Data.ResponseObject;

public class ResponseSupplies
{
    public List<ResponseSupplyObject> responseSupplies { get; set; }
}

public class ResponseSupplyObject
{
    public string? Name { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public TimeSpan CookingTime { get; set; }
    public String Category { get; set; }
}
