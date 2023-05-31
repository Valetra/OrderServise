using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using Constants;
using apiForRadBot.Data.ResponseObject;
using Newtonsoft.Json;

namespace BotSettings;
public class RadBot
{
    public static readonly ReplyKeyboardMarkup REPLY_KEYBOARD_MARKUP = new(new[]
    {
    new KeyboardButton[] { BotMenuButtons.showMenu },
    new KeyboardButton[] { BotMenuButtons.makeOrder },
    new KeyboardButton[] { BotMenuButtons.showLocation },
    new KeyboardButton[] { BotMenuButtons.showContact },
})
    {
        ResizeKeyboard = true
    };

    public static async Task Start(ITelegramBotClient _, long chatId, CancellationToken cancellationToken)
    {
        await _.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Добро пожаловать в наш чат-бот!\nПользование данным ботом происходит с помощью навигационной панели.",
                        disableNotification: true,
                        replyMarkup: REPLY_KEYBOARD_MARKUP,
                        cancellationToken: cancellationToken);
    }
    public static async Task MakeOrder(ITelegramBotClient _, long chatId, CancellationToken cancellationToken)
    {
        await _.SendTextMessageAsync(
            chatId: chatId,
            text: $"Выберите раздел",
            parseMode: ParseMode.MarkdownV2,
            disableNotification: true,
            replyMarkup: InlineKeybordButtons.categories,
            cancellationToken: cancellationToken);
    }

    public static async void ShowMenu(ITelegramBotClient _, long chatId, CancellationToken cancellationToken)
    {
        HttpClient httpClient = new HttpClient();

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5132/supply");

        using HttpResponseMessage response = await httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            string jsonResponseContent = await response.Content.ReadAsStringAsync();
            ResponseSupplies responseObject = JsonConvert.DeserializeObject<ResponseSupplies>(jsonResponseContent);

            string responseMenu = "";

            foreach (var item in responseObject.responseSupplies)
            {
                responseMenu += $"Блюдо: {item.Name} | Цена: {item.Price}\n";
            }
            await _.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"{responseMenu}",
                    disableNotification: true,
                    replyMarkup: REPLY_KEYBOARD_MARKUP,
                    cancellationToken: cancellationToken);
        }
    }

    public static async void ShowContact(ITelegramBotClient _, long chatId, CancellationToken cancellationToken)
    {
        await _.SendContactAsync(
                chatId: chatId,
                phoneNumber: "+7-(902)-430-67-62",
                firstName: "RAD",
                lastName: "Пиво и бургеры",
                cancellationToken: cancellationToken);
    }

    public static async void ShowLocation(ITelegramBotClient _, long chatId, CancellationToken cancellationToken)
    {
        await _.SendVenueAsync(
                      chatId: chatId,
                      latitude: 56.632975600072584,
                      longitude: 47.89161568123221,
                      title: "RAD. Бургеры и пиво",
                      address: "ул. Пушкина, 19, Йошкар-Ола, Респ. Марий Эл, 424000",
                      cancellationToken: cancellationToken);
    }
}