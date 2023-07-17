using System.Net.Http.Json;
using Models;

namespace ChatBot.Managers;

internal class OrderManager
{
    public static async Task<(Guid, int)> PostOrderToAPI(ControllerManager controllerManager, Order order)
    {
        HttpClient httpClient = new HttpClient();

        JsonContent content = JsonContent.Create(order);

        var responceMesage = await httpClient.PostAsync(controllerManager.GetOrderURL(), content);

        var absolutePath = responceMesage.Headers.Location.AbsolutePath;
        var query = responceMesage.Headers.Location.Query;

        Guid orderGuid = Guid.Parse(absolutePath.Replace("/Order/", ""));
        int orderNumber = Int32.Parse(query.Replace($"?number=", ""));

        return (orderGuid, orderNumber);
    }

}
