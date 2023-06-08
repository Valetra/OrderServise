using Contracts;
using Models;
using Newtonsoft.Json;

namespace ChatBot.Managers;

public class CategoryManager
{
    public static async Task<List<ICategory>> GetCategoriesFromAPI(string apiPath)
    {
        string controllerName = "category";

        HttpClient httpClient = new HttpClient();

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, apiPath + controllerName);

        using HttpResponseMessage response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Category>>(responseString)?.ToList<ICategory>() ?? new();
        }
        else
        {
            return new();
        }
    }
}
