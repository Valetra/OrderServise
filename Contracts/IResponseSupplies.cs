using apiForRadBot.Data.ResponseObject;

namespace Contracts;

public interface IResponseSupplies
{
    List<ResponseSupplyObject> ResponseSupplies { get; set; }
}