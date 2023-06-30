using RESTful_API.Data.Models;
using RESTful_API.Data.ResponseObject;
using Contracts;

namespace RESTful_API.Core.Mapper;

public class SupplyExtensions
{
    public static List<ResponseSupplyObject> ToResponseSupplies(List<Supply> supplies, List<Category> categories)
    {
        List<ResponseSupplyObject> result = new();

        var groupedSupplies = supplies.GroupBy(s => s.Id);

        foreach (var supplyGroup in groupedSupplies)
        {
            Supply supply = supplyGroup.First();

            result.Add(new ResponseSupplyObject
            {
                Id = supply.Id,
                Name = supply.Name,
                Count = supplyGroup.Count(),
                Price = supply.Price,
                CookingTime = supply.CookingTime,
                CategoryId = supply.CategoryId
            });
        }
        return result;
    }
}
