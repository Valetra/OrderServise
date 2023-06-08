using apiForRadBot.Data.Models;
using apiForRadBot.Data.RequestObject;

namespace apiForRadBot.Core.Mapper;

public static class CategoryExtensions
{
    public static Category ToCategory(this PostCategoryObject category) => new()
    {
        Name = category.Name
    };

    public static Category ToCategory(this PutCategoryObject category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };
}
