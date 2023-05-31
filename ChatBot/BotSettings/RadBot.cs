using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

using Constants;
using apiForRadBot.Data.ResponseObject;
using BotSettings;
using Newtonsoft.Json;

public class RadBot
{
    private static readonly ReplyKeyboardMarkup REPLY_KEYBOARD_MARKUP = new(new[]
    {
    new KeyboardButton[] { BotMenuButtons.showMenu },
    new KeyboardButton[] { BotMenuButtons.makeOrder },
    new KeyboardButton[] { BotMenuButtons.showLocation },
    new KeyboardButton[] { BotMenuButtons.showContact },
})
    {
        ResizeKeyboard = true
    };

    private readonly TelegramBotClient _client;

    public RadBot(string token, CancellationToken cancellationToken)
    {
        _client = new TelegramBotClient(token);

        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }
        };

        _client.StartReceiving(
            HandleUpdateAsync,
            HandlePoolingErrorAsync,
            receiverOptions,
            cancellationToken
        );
    }

    public async Task<string?> GetUsername()
    {
        var me = await _client.GetMeAsync();

        return me.Username;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
    {
        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text;
        string firstName = update.Message.From.FirstName;
        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        switch (messageText)
        {
            case "/start":
                await Start(chatId, cancellationToken);
                break;
            case BotMenuButtons.makeOrder:
                await MakeOrder(chatId, cancellationToken);
                break;
            case BotMenuButtons.showMenu:
                ShowMenu(chatId, cancellationToken);
                break;
            case BotMenuButtons.showContact:
                ShowContact(chatId, cancellationToken);
                break;
            case BotMenuButtons.showLocation:
                ShowLocation(chatId, cancellationToken);
                break;
        }
    }

    if (update.CallbackQuery != null)
    {
        string callbackData = update.CallbackQuery.Data;
        string actionText = "default";
        InlineKeyboardMarkup buttons = null;

        if (callbackData == "burgers")
        {
            actionText = "Выберите бургер";
            buttons = InlineKeyboardButtons.burgers;
        }
        else if (callbackData == "beer")
        {
            actionText = "Выберите пиво";
            buttons = InlineKeyboardButtons.beer;

        }
        else if (callbackData == "categories")
        {
            actionText = "Выберите категорию";
            buttons = InlineKeyboardButtons.categories;
        }
        else if (callbackData == "drinksNA")
        {
            actionText = "Выберите напиток";
            buttons = InlineKeyboardButtons.drinksNA;
        }
        else if (callbackData == "cancelOrder")
        {
            actionText = "Заказ был отменен";
            buttons = null;
        }
        else
        {
            actionText = "Else,какая-то из кнопок не обработана!";
            buttons = InlineKeyboardButtons.categories;
        }
        await CallbackAction(botClient, update, cancellationToken, actionText, buttons);
    }
}

private async Task<Message> CallbackAction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string actionText, InlineKeyboardMarkup buttons)
{
    return await botClient.EditMessageTextAsync(
            messageId: update.CallbackQuery.Message.MessageId,
            chatId: update.CallbackQuery.Message.Chat.Id,
            text: actionText,
            replyMarkup: buttons,
            cancellationToken: cancellationToken);
}

    private Task HandlePoolingErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
   {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    private async Task Start(long chatId, CancellationToken cancellationToken)
    {
        await _client.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Добро пожаловать в наш чат-бот!\nПользование данным ботом происходит с помощью навигационной панели.",
                        disableNotification: true,
                        replyMarkup: REPLY_KEYBOARD_MARKUP,
                        cancellationToken: cancellationToken);
    }

    private async void ShowMenu(long chatId, CancellationToken cancellationToken)
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
            await _client.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"{responseMenu}",
                    disableNotification: true,
                    replyMarkup: REPLY_KEYBOARD_MARKUP,
                    cancellationToken: cancellationToken);
        }
    }

    private async void ShowContact(long chatId, CancellationToken cancellationToken)
    {
        await _client.SendContactAsync(
                chatId: chatId,
                phoneNumber: "+7-(902)-430-67-62",
                firstName: "RAD",
                lastName: "Пиво и бургеры",
                cancellationToken: cancellationToken);
    }

    private async void ShowLocation(long chatId, CancellationToken cancellationToken)
    {
        await _client.SendVenueAsync(
                      chatId: chatId,
                      latitude: 56.632975600072584,
                      longitude: 47.89161568123221,
                      title: "RAD. Бургеры и пиво",
                      address: "ул. Пушкина, 19, Йошкар-Ола, Респ. Марий Эл, 424000",
                      cancellationToken: cancellationToken);
    }

    private async Task MakeOrder(long chatId, CancellationToken cancellationToken)
    {
        await _client.SendTextMessageAsync(
            chatId: chatId,
            text: $"Выберите раздел",
            parseMode: ParseMode.MarkdownV2,
            disableNotification: true,
            replyMarkup: InlineKeyboardButtons.categories,
            cancellationToken: cancellationToken);
    }
}
