using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using private_data;
using myLib;

//Инициализация меню
List<Supply> allSupplies = new List<Supply>();

Supply food = new Supply("Бургер: Рад", 380);
Supply drink = new Supply("Пиво: Апа", 190);
Supply otherFood = new Supply("Компот", 60);

//Добавление объектов в список меню
allSupplies.Add(food);
allSupplies.Add(drink);
allSupplies.Add(otherFood);

async static void ShowMenu(List<Supply> supplies, ITelegramBotClient botClient, long chatId, ReplyKeyboardMarkup replyKeyboardMarkup, CancellationToken cancellationToken)
{
    foreach (var supply in supplies)
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: $"{supply.Name} | цена = {supply.Coast}р",
            disableNotification: true,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}

MyPrivateData myData = new MyPrivateData();

var botClient = new TelegramBotClient(myData.myToken);

using CancellationTokenSource cts = new();

//Custom buttons
Buttons buttons = new Buttons();

ReplyKeyboardMarkup replyKeyboardMarkup = new(new[]
{
    new KeyboardButton[] { buttons.button1 },
    new KeyboardButton[] { buttons.button4 },
    new KeyboardButton[] { buttons.button3 },
    new KeyboardButton[] { buttons.button2 },
    new KeyboardButton[] { buttons.button5 },
})
{
    ResizeKeyboard = true
};

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");

Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Message is not { } message)
        return;
    // Only process text messages
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    if (messageText == buttons.button1)
    {
        ShowMenu(allSupplies, botClient, chatId, replyKeyboardMarkup, cancellationToken);
    }
    else if (messageText == buttons.button2)
    {
        Message sentmessage = await botClient.SendContactAsync(
            chatId: chatId,
            phoneNumber: "+7-(902)-430-67-62",
            firstName: "RAD",
            lastName: "Пиво и бургеры",
            cancellationToken: cancellationToken);

    }
    else if (messageText == buttons.button3)
    {
        Message sentmessage = await botClient.SendVenueAsync(
            chatId: chatId,
            latitude: 56.632975600072584,
            longitude: 47.89161568123221,
            title: "RAD. Бургеры и пиво",
            address: "ул. Пушкина, 19, Йошкар-Ола, Респ. Марий Эл, 424000",
            cancellationToken: cancellationToken);

    }
    else if (messageText == buttons.button4)
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Make your order here:",
            disableNotification: true,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
    else
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "refresh",
            disableNotification: true,
            replyMarkup: replyKeyboardMarkup,
            cancellationToken: cancellationToken);
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

public struct Buttons
{
    public string button1;
    public string button2;
    public string button3;
    public string button4;
    public string button5;

    //Инициализация кнопок
    public Buttons()
    {
        button1 = "Show menu";
        button2 = "Our contact";
        button3 = "Where are we located";
        button4 = "Make an order";
        button5 = "refresh";
    }
}