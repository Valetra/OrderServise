using apiForRadBot.Data.Models;
using apiForRadBot.Data.RequestObject;
using apiForRadBot.Data.ResponseObject;
using System.Text.RegularExpressions;

namespace apiForRadBot.Core.Mapper;

public static class OrderExtensions
{
    public static List<Supply> ToSuppliesList(PostOrderObject suppliesId)
    {
        List<Supply> supplies = new List<Supply>();

        for (int i = 0; i < suppliesId.SuppliesId.Count; i++)
        {
            supplies.Add(new Supply { Id = suppliesId.SuppliesId[i], Name = null, Price = 0, CookingTime = null });
        }

        return supplies;
    }
    public static ResponseOrderObject ToResponseObject(Order order)
    {
        ResponseOrderObject responseOrderObject = new ResponseOrderObject();

        responseOrderObject.Status = order.Status;
        responseOrderObject.Payed = order.Payed;
        responseOrderObject.Supply = null;

        return responseOrderObject;
    }
}
