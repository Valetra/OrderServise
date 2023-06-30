namespace ChatBot.Managers;

public class ControllerManager
{
    private readonly string _apiUrl;

    public string GetSupplyURL()
    {
        return $"{_apiUrl}Supply";
    }
    public string GetOrderURL()
    {
        return $"{_apiUrl}Order";
    }
    public string GetCategoryURL()
    {
        return $"{_apiUrl}Category";
    }

    public ControllerManager(string apiUrl)
    {
        _apiUrl = apiUrl;
    }
}