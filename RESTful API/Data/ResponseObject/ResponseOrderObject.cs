namespace apiForRadBot.Data.ResponseObject;

public class ResponseOrderObject
{
    public string Status { get; set; }
    public bool Payed { get; set; }
    public DateTime CreateDateTime { get; set; }
    public int Number { get; set; }//в сервисе вычислить с помощью LINQ по CreateDateTime за сутки
    public List<ResponseSupplyObject>? Supplies { get; set; }
}
