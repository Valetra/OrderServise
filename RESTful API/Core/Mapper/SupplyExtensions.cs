using apiForRadBot.Data.Models;
using apiForRadBot.Data.ResponseObject;

namespace apiForRadBot.Core.Mapper;

public class SupplyExtensions
{
    public static List<ResponseSupplyObject> ToResponseSupplies(List<Supply> supplies, List<Category> categories)
    {
        List<ResponseSupplyObject> result = new();

        var groupedSupplies = supplies.GroupBy(i => i.Name);
        string categoryName;

        foreach (var supply in groupedSupplies)
        {
            categoryName = categories.FirstOrDefault(c => c.Id == supplies.FirstOrDefault(s => s.Name == supply.Key).CategoryId).Name;

            result.Add(new ResponseSupplyObject
            {
                Name = supply.Key,
                Count = supply.Count(),
                Price = supplies.FirstOrDefault(s => s.Name == supply.Key).Price,
                CookingTime = supplies.FirstOrDefault(s => s.Name == supply.Key).CookingTime,
                Category = categoryName
            });
        }
        return result;
    }
}
