namespace RESTful_API.Data.RequestObject;

public class PutOrderObject
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public bool Payed { get; set; }
}
