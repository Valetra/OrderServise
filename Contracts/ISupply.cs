namespace Contracts;

public interface ISupply
{
    Guid Id { get; set; }
    string? Name { get; set; }
    int Count { get; set; }
    int Price { get; set; }
    TimeSpan CookingTime { get; set; }
    String Category { get; set; }
}

public class Supply : ISupply
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public TimeSpan CookingTime { get; set; }
    public String Category { get; set; }
}