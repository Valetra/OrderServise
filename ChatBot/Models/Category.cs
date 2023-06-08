using Contracts;

namespace Models;

public class Category : ICategory
{
    public Guid Id { get; init; }
    public string Name { get; init; }
}