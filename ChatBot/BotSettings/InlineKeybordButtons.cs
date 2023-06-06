using ChatBot.Managers;
using Contracts;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotSettings;

public class InlineKeyboardButtons
{
    private readonly string _apiPath;
    private readonly List<Category> _categories;

    public InlineKeyboardButtons(string apiPath, List<Category> categories)
    {
        _apiPath = apiPath;
        _categories = categories;
    }

    public InlineKeyboardMarkup? GetCategoryButtons()
    {
        InlineKeyboardMarkup inlineKeyboardMarkup;

        List<InlineKeyboardButton>? categoryButtons = new();
        List<InlineKeyboardButton>? cancelButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "отмена заказа", callbackData: "cancelOrder")
        };

        if (_categories != null)
        {
            foreach (var category in _categories)
            {
                categoryButtons.Add(InlineKeyboardButton.WithCallbackData(text: category.Name, callbackData: category.Name));
            }
            inlineKeyboardMarkup = new(new[]
            {
                categoryButtons,
                cancelButton
            });
            return inlineKeyboardMarkup;
        }
        else
            return null;

    }

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
