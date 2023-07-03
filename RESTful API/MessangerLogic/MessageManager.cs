using private_data;

namespace RESTful_API.MessangerLogic;

internal class MessageManager
{
    private static readonly string telegramApiUrl = "https://api.telegram.org/";
    private static readonly MyPrivateData botToken = new();

    public static async Task<HttpResponseMessage> SendMessageToChat(Message message)
    {
        string requestUri = $"{telegramApiUrl}bot{botToken.TelegramBotToken}/sendMessage";

        HttpClient httpClient = new HttpClient();

        var content = JsonContent.Create(message);

        return await httpClient.PostAsync(requestUri, content);
    }

}

