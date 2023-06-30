using Contracts;

namespace RESTful_API.Data.ResponseObject;

public class ResponseCategoryObject : ICategory
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
