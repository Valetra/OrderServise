using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using myLib;
using Constants;

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

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        ReceiverOptions receiverOptions = new()
        {
            AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
        };

        _client.StartReceiving(
          updateHandler: HandleUpdateAsync,
          pollingErrorHandler: HandlePollingErrorAsync,
          receiverOptions: receiverOptions,
          cancellationToken: cancellationToken
        );
    }

    public async Task<string?> GetUsername()
    {
        var me = await _client.GetMeAsync();

        return me.Username;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        // Only process Message updates: https://core.telegram.org/bots/api#message
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        switch (messageText)
        {
            case "/start":
                await Start(chatId, cancellationToken);
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
            case BotMenuButtons.makeOrder:
                MakeOrder(chatId, cancellationToken);
                break;
        }
    }

    private Task HandlePollingErrorAsync(ITelegramBotClient _, Exception exception, CancellationToken cancellationToken)
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
                        text: "Hello, let`s make an order",
                        disableNotification: true,
                        replyMarkup: REPLY_KEYBOARD_MARKUP,
                        cancellationToken: cancellationToken);
    }

    private async void ShowMenu(long chatId, CancellationToken cancellationToken)
    {
        //Инициализация меню
        List<Supply> supplies = new List<Supply>();

        Supply food = new Supply("Бургер: Рад", 380);
        Supply drink = new Supply("Пиво: Апа", 190);
        Supply otherFood = new Supply("Компот", 60);

        //Добавление объектов в список меню
        supplies.Add(food);
        supplies.Add(drink);
        supplies.Add(otherFood);

        foreach (var supply in supplies)
        {
            Message sentMessage = await _client.SendTextMessageAsync(
                chatId: chatId,
                text: $"{supply.Name} | цена = {supply.Price} ₽",
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

    private async void MakeOrder(long chatId, CancellationToken cancellationToken)
    {

        InlineKeyboardMarkup inlineKeyboard = new(new[]
                     {
                    // Первая строка
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Бургеры", callbackData: "burgers"),
                        InlineKeyboardButton.WithCallbackData(text: "Пиво", callbackData: "beer"),
                        InlineKeyboardButton.WithCallbackData(text: "Напитки б/а", callbackData: "drinksN/A"),
                    },
                    // Вторая строка
                    new []
                    {
                        InlineKeyboardButton.WithCallbackData(text: "отмена заказа", callbackData: "cancelOrder"),
                    },
                });

        await _client.SendTextMessageAsync(
            chatId: chatId,
            text: "Выберите категорию:",
            parseMode: ParseMode.MarkdownV2,
            disableNotification: true,
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }
}