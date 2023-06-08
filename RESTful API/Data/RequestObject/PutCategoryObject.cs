namespace apiForRadBot.Data.RequestObject;

public class PutCategoryObject
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
