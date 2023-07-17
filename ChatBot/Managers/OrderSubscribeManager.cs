using System.Net.Http.Json;
using Models;

namespace ChatBot.Managers;

internal class OrderSubscribeManager
{
    public static async Task PostOrderSubscribeToAPI(ControllerManager controllerManager, OrderSubscribe orderSubscribe)
    {
        HttpClient httpClient = new HttpClient();

        JsonContent content = JsonContent.Create(orderSubscribe);

        await httpClient.PostAsync(controllerManager.GetOrderSubscribeURL(), content);
    }
}
