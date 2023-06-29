namespace ChatBot.Managers;

public class ControllerManager
{
    private readonly string _apiUrl;

    public string GetSupplyURL()
    {
        return $"{_apiUrl}supply";
    }
    public string GetOrderURL()
    {
        return $"{_apiUrl}order";
    }
    public string GetCategoryURL()
    {
        return $"{_apiUrl}category";
    }

    public ControllerManager(string apiUrl)
    {
        _apiUrl = apiUrl;
    }
}