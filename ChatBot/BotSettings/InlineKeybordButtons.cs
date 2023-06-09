using Contracts;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotSettings;

public class InlineKeyboardButtons
{
    private readonly string _apiPath;
    private readonly bool _isOrderEmpty;
    private readonly List<ICategory> _categories;
    private readonly List<ISupply> _supplies;

    public InlineKeyboardButtons(string apiPath, List<ICategory> categories, List<ISupply> supplies, bool isOrderEmpty)
    {
        _apiPath = apiPath;
        _categories = categories;
        _supplies = supplies;
        _isOrderEmpty = isOrderEmpty;
    }

    public InlineKeyboardMarkup GetCategoryButtons()
    {
        List<InlineKeyboardButton> categoryButtons = _categories
            .Select(c => InlineKeyboardButton.WithCallbackData(text: c.Name, callbackData: c.Id.ToString()))
            .ToList();

        List<InlineKeyboardButton> AcceptOrderButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "Подтвердить заказ", callbackData: "accept")
        };
        List<InlineKeyboardButton> cancelButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "отмена заказа", callbackData: "cancel")
        };

        if (_isOrderEmpty)
        {
            return new(new[]
            {
                categoryButtons,
                cancelButton
            });
        }
        return new(new[]
        {
            categoryButtons,
            AcceptOrderButton,
            cancelButton
        });
    }

    public InlineKeyboardMarkup GetCategorySuppliesButtons(Guid categoryId)
    {
        List<InlineKeyboardButton> subcategoryButtons = _supplies
            .Where(s => s.CategoryId == categoryId)
            .Select(s => InlineKeyboardButton.WithCallbackData(text: s.Name, callbackData: s.Id.ToString()))
            .ToList();

        List<InlineKeyboardButton> backButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "back")
        };

        return new(new[]
        {
            subcategoryButtons,
            backButton,
        });
    }
}
