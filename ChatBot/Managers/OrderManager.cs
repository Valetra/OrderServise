using System.Net.Http.Json;
using Models;

namespace ChatBot.Managers;

internal class OrderManager
{
    public static async Task PostOrderToAPI(ControllerManager controllerManager, Order order)
    {
        HttpClient httpClient = new HttpClient();

        JsonContent content = JsonContent.Create(order);

        using var response = await httpClient.PostAsync(controllerManager.GetOrderURL(), content);
    }
}
