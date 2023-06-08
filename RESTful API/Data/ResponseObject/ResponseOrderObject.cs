namespace apiForRadBot.Data.ResponseObject;

public class ResponseOrderObject
{
    public string Status { get; set; }
    public bool Payed { get; set; }
    public List<ResponseSupplyObject>? Supplies { get; set; }
}
