using Contracts;

namespace apiForRadBot.Data.ResponseObject;

public class ResponseCategoryObject : ICategory
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}
