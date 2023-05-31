using private_data;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using BotSettings;
using Constants;

MyPrivateData myData = new MyPrivateData();

CancellationTokenSource cancellationTokenSource = new();

var botClient = new TelegramBotClient(myData.TelegramBotToken);

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { }
};
botClient.StartReceiving(
    HandleUpdateAsync,
    HandleErrorAsync,
    receiverOptions,
    cancellationToken: cancellationTokenSource.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");

Console.ReadLine();

cancellationTokenSource.Cancel();
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
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
                RadBot.Start(botClient, chatId, cancellationToken);
                break;
            case BotMenuButtons.makeOrder:
                await RadBot.MakeOrder(botClient, chatId, cancellationToken);
                break;
            case BotMenuButtons.showMenu:
                RadBot.ShowMenu(botClient, chatId, cancellationToken);
                break;
            case BotMenuButtons.showContact:
                RadBot.ShowContact(botClient, chatId, cancellationToken);
                break;
            case BotMenuButtons.showLocation:
                RadBot.ShowLocation(botClient, chatId, cancellationToken);
                break;
        }
    }

    if (update.CallbackQuery != null)
    {
        string callbackData = update.CallbackQuery.Data;
        string actionText = "default";
        InlineKeyboardMarkup buttons = InlineKeybordButtons.categories;

        if (callbackData == "burgers")
        {
            actionText = "Выберите бургер";
            buttons = InlineKeybordButtons.burgers;
        }
        else if (callbackData == "beer")
        {
            actionText = "Выберите пиво";
            buttons = InlineKeybordButtons.beer;

        }
        else if (callbackData == "categories")
        {
            actionText = "Выберите категорию";
            buttons = InlineKeybordButtons.categories;
        }
        else if (callbackData == "drinksNA")
        {
            actionText = "Выберите напиток";
            buttons = InlineKeybordButtons.drinksNA;
        }
        else
        {
            actionText = "Else что-то не обработано!";
            buttons = InlineKeybordButtons.categories;
        }
        await CallbackAction(botClient, update, cancellationToken, actionText, buttons);

    }
}

async Task<Message> CallbackAction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken, string actionText, InlineKeyboardMarkup buttons)
{
    return await botClient.EditMessageTextAsync(
            messageId: update.CallbackQuery.Message.MessageId,
            chatId: update.CallbackQuery.Message.Chat.Id,
            text: actionText,
            replyMarkup: buttons,
            cancellationToken: cancellationToken);
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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

