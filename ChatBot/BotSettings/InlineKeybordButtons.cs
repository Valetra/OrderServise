using ChatBot.Managers;
using Contracts;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotSettings;

public class InlineKeyboardButtons
{
    private readonly string _apiPath;
    private readonly List<Category> _categories;
    private readonly List<Supply> _supplies;

    public InlineKeyboardButtons(string apiPath, List<Category> categories, List<Supply> supplies)
    {
        _apiPath = apiPath;
        _categories = categories;
        _supplies = supplies;
    }

    public InlineKeyboardMarkup? GetCategoryButtons()
    {
        List<InlineKeyboardButton>? categoryButtons = new();
        List<InlineKeyboardButton>? cancelButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "отмена заказа", callbackData: "cancel")
        };

        if (_categories != null)
        {
            foreach (var category in _categories)
            {
                categoryButtons.Add(InlineKeyboardButton.WithCallbackData(text: category.Name, callbackData: category.Name));
            }
            return new(new[]
            {
                categoryButtons,
                cancelButton
            });
        }
        else
            return null;
    }
    public InlineKeyboardMarkup? GetCategorySuppliesButtons(string category)
    {
        List<InlineKeyboardButton>? subcategoryButtons = new();
        List<InlineKeyboardButton>? backButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "back")
        };

        foreach (var supply in _supplies)
        {
            if (supply.Category == category)
                subcategoryButtons.Add(InlineKeyboardButton.WithCallbackData(text: supply.Name, callbackData: supply.Name));
        }
        return new(new[]
            {
                subcategoryButtons,
                backButton
            });
    }
}
