using apiForRadBot.Data.Models;

namespace apiForRadBot.Data.ResponseObject;

public class ResponseOrderObject
{
    public string Status { get; set; }
    public bool Payed { get; set; }
    public List<Supply> Supply { get; set; }
}
