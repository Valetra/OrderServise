namespace Contracts;

public interface ICategory
{
    Guid Id { get; set; }
    string Name { get; set; }
}

public class Category : ICategory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
