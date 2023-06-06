using Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using Telegram.Bots.Types;

namespace ChatBot.Managers;
public class SupplyManager
{
    public static async Task<List<Supply>?> GetSuppliesFromAPI(string apiPath)
    {
        string controllerName = "supply";

        HttpClient httpClient = new HttpClient();

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiPath + controllerName);

        using HttpResponseMessage response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            return JsonConvert.DeserializeObject<List<Supply>>(await response.Content.ReadAsStringAsync());
        else
            return null;
    }
}
