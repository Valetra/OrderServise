using System.Net.Http.Json;
using Models;

namespace ChatBot.Managers;

internal class OrderManager
{
    public static async Task<Guid> PostOrderToAPI(ControllerManager controllerManager, Order order)
    {
        HttpClient httpClient = new HttpClient();

        JsonContent content = JsonContent.Create(order);

        var responceMesage = await httpClient.PostAsync(controllerManager.GetOrderURL(), content);

        var uri = responceMesage.Headers.Location.AbsoluteUri;

        return new(uri.Replace($"{controllerManager.GetOrderURL()}/", ""));
    }
}
