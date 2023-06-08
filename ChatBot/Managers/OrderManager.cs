using System.Net.Http.Json;
using Models;

namespace ChatBot.Managers;

internal class OrderManager
{
    public static async Task PostOrderToAPI(string apiPath, Order order)
    {
        string controllerName = "order";

        HttpClient httpClient = new HttpClient();

        JsonContent content = JsonContent.Create(order);

        using var response = await httpClient.PostAsync(apiPath + controllerName, content);
    }
}
