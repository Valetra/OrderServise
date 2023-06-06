using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Contracts;
using Constants;
using BotSettings;
using Newtonsoft.Json;
using ChatBot.Managers;

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
    private readonly string _apiPath;

    public RadBot(string token, CancellationToken cancellationToken, string apiPath)
    {
        _client = new TelegramBotClient(token);
        _apiPath = apiPath;

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

    private async Task ShowMenu(long chatId, CancellationToken cancellationToken)
    {
        List<ResponseSupply>? supplies = await SupplyManager.GetSuppliesFromAPI(_apiPath);
        string? responseMenu = null;

        if (supplies != null)
            foreach (var item in supplies)
                responseMenu += $"{item.Name} \t {item.Price}₽\n";
        else
            responseMenu += "В меню пусто.";

        await _client.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"{responseMenu}",
                    disableNotification: true,
                    replyMarkup: REPLY_KEYBOARD_MARKUP,
                    cancellationToken: cancellationToken);
    }

    public async Task<string?> GetUsername()
    {
        var me = await _client.GetMeAsync();

        return me.Username;
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        List<Category>? categories = await CategoryManager.GetCategoriesFromAPI(_apiPath);

        if (update.Type == UpdateType.Message && update.Message!.Type == MessageType.Text)
        {
            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            string firstName;
            if (update.Message.From is not null)
                firstName = update.Message.From.FirstName;
            else
                firstName = "Незнакомец";

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            switch (messageText)
            {
                case "/start":
                    await Start(firstName, chatId, cancellationToken);
                    break;
                case BotMenuButtons.makeOrder:
                    await MakeOrder(chatId, cancellationToken, categories);
                    break;
                case BotMenuButtons.showMenu:
                    await ShowMenu(chatId, cancellationToken);
                    break;
                case BotMenuButtons.showContact:
                    await ShowContact(chatId, cancellationToken);
                    break;
                case BotMenuButtons.showLocation:
                    await ShowLocation(chatId, cancellationToken);
                    break;
            }
        }
        if (update.CallbackQuery != null)
            await HandleCallbackQuery(botClient, update.CallbackQuery, cancellationToken);
    }

    private async Task<Message> HandleCallbackQuery(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        List<Category>? categories = await CategoryManager.GetCategoriesFromAPI(_apiPath);
        string? callbackData = callbackQuery.Data;
        string actionText;
        InlineKeyboardMarkup? buttons;
        InlineKeyboardButtons newButtons = new InlineKeyboardButtons(_apiPath, categories);


        switch (callbackData)
        {
            case "burgers":
                actionText = "Выберите бургер";
                buttons = InlineKeyboardButtons.burgers;
                break;
            case "beer":
                actionText = "Выберите пиво";
                buttons = InlineKeyboardButtons.beer;
                break;
            case "drinksNA":
                actionText = "Выберите напиток";
                buttons = InlineKeyboardButtons.drinksNA;
                break;
            case "categories":
                actionText = "Выберите категорию";
                buttons = newButtons.GetCategoryButtons();
                break;
            case "cancelOrder":
                actionText = "Заказ был отменен";
                buttons = null;
                break;
            default:
                actionText = "Else,какая-то из кнопок не обработана!";
                buttons = newButtons.GetCategoryButtons();
                break;
        }
        return await CallbackAction(botClient, callbackQuery, actionText, buttons, cancellationToken);
    }
    private async Task<Message> CallbackAction(ITelegramBotClient botClient, CallbackQuery callbackQuery, string actionText, InlineKeyboardMarkup? buttons, CancellationToken cancellationToken)
    {
        return await botClient.EditMessageTextAsync(
            messageId: callbackQuery.Message.MessageId,
            chatId: callbackQuery.Message.Chat.Id,
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

    private async Task Start(string firstName, long chatId, CancellationToken cancellationToken)
    {
        await _client.SendTextMessageAsync(
                        chatId: chatId,
                        text: $"{firstName}, добро пожаловать в наш чат-бот!\nПользование данным ботом происходит с помощью навигационной панели.",
                        disableNotification: true,
                        replyMarkup: REPLY_KEYBOARD_MARKUP,
                        cancellationToken: cancellationToken);
    }

    private async Task ShowContact(long chatId, CancellationToken cancellationToken)
    {
        await _client.SendContactAsync(
                chatId: chatId,
                phoneNumber: "+7-(902)-430-67-62",
                firstName: "RAD",
                lastName: "Пиво и бургеры",
                cancellationToken: cancellationToken);
    }

    private async Task ShowLocation(long chatId, CancellationToken cancellationToken)
    {
        await _client.SendVenueAsync(
                      chatId: chatId,
                      latitude: 56.632975600072584,
                      longitude: 47.89161568123221,
                      title: "RAD. Бургеры и пиво",
                      address: "ул. Пушкина, 19, Йошкар-Ола, Респ. Марий Эл, 424000",
                      cancellationToken: cancellationToken);
    }

    private async Task MakeOrder(long chatId, CancellationToken cancellationToken, List<Category>? categories)
    {
        InlineKeyboardButtons newButtons = new InlineKeyboardButtons(_apiPath, categories);

        await _client.SendTextMessageAsync(
            chatId: chatId,
            text: $"Выберите раздел",
            parseMode: ParseMode.MarkdownV2,
            disableNotification: true,
            replyMarkup: newButtons.GetCategoryButtons(),
            cancellationToken: cancellationToken);
    }
}
