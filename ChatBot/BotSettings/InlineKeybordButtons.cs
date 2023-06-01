using Telegram.Bot.Types.ReplyMarkups;

namespace BotSettings;

public class InlineKeyboardButtons
{
    public static InlineKeyboardMarkup categories = new(new[]
    {
        // Первая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Бургеры", callbackData: "burgers"),
            InlineKeyboardButton.WithCallbackData(text: "Пиво", callbackData: "beer"),
            InlineKeyboardButton.WithCallbackData(text: "Напитки б/а", callbackData: "drinksNA"),
        },
        // Вторая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "отмена заказа", callbackData: "cancelOrder"),
        },
    });

    public static InlineKeyboardMarkup burgers = new(new[]
    {
        // Первая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Бургер 1", callbackData: "burger1"),
            InlineKeyboardButton.WithCallbackData(text: "Бургер 2", callbackData: "burger2"),
            InlineKeyboardButton.WithCallbackData(text: "Бургер 3", callbackData: "burger3"),
        },
        // Вторая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "назад", callbackData: "categories"),
        },
    });

    public static InlineKeyboardMarkup beer = new(new[]
    {
        // Первая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Пиво 1", callbackData: "beer1"),
            InlineKeyboardButton.WithCallbackData(text: "Пиво 2", callbackData: "beer2"),
            InlineKeyboardButton.WithCallbackData(text: "Пиво 3", callbackData: "beer3"),
        },
        // Вторая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "назад", callbackData: "categories"),
        },
    });

    public static InlineKeyboardMarkup drinksNA = new(new[]
    {
        // Первая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "Кофе", callbackData: "сoffe"),
            InlineKeyboardButton.WithCallbackData(text: "Компот", callbackData: "compote"),
            InlineKeyboardButton.WithCallbackData(text: "Чай", callbackData: "tea"),
        },
        // Вторая строка
        new []
        {
            InlineKeyboardButton.WithCallbackData(text: "назад", callbackData: "categories"),
        },
    });
}
