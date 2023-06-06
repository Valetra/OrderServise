using Contracts;
using Newtonsoft.Json;

namespace ChatBot.Managers;

public class CategoryManager
{
    public static async Task<List<Category>?> GetCategoriesFromAPI(string apiPath)
    {
        string controllerName = "category";

        HttpClient httpClient = new HttpClient();

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiPath + controllerName);

        using HttpResponseMessage response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
            return JsonConvert.DeserializeObject<List<Category>>(await response.Content.ReadAsStringAsync());
        else
            return null;
    }
}
