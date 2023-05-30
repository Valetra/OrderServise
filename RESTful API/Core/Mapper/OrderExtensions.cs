using apiForRadBot.Data.Models;
using apiForRadBot.Data.RequestObject;

namespace apiForRadBot.Core.Mapper;

public static class OrderExtensions
{
    public static List<Supply> ToSuppliesList(PostOrderObject suppliesId)
    {
        List<Supply> supplies = new List<Supply>();

        for (int i = 0; i < suppliesId.SuppliesId.Count; i++)
        {
            supplies.Add(new Supply { Id = suppliesId.SuppliesId[i], Name = null, Price = 0, CookingTime = TimeSpan.Parse("00:00:00") });
        }
        return supplies;
    }
}
