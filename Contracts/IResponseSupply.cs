namespace Contracts;

public interface IResponseSupply
{
    string? Name { get; set; }
    int Count { get; set; }
    int Price { get; set; }
    TimeSpan CookingTime { get; set; }
    String Category { get; set; }
}

public class ResponseSupply : IResponseSupply
{
    public string? Name { get; set; }
    public int Count { get; set; }
    public int Price { get; set; }
    public TimeSpan CookingTime { get; set; }
    public String Category { get; set; }
}