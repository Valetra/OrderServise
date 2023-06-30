using RESTful_API.Data.Models;
using RESTful_API.Data.RequestObject;

namespace RESTful_API.Core.Mapper;

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
