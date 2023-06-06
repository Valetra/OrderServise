using Contracts;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ChatBot.Managers;

internal class OrderManager
{
    public static async Task<bool> PostOrderToAPI(string apiPath, Order order)
    {
        string controllerName = "order";

        HttpClient httpClient = new HttpClient();

        JsonContent content = JsonContent.Create(order);

        using var response = await httpClient.PostAsync(apiPath + controllerName, content);

        return response.IsSuccessStatusCode;
    }
}
