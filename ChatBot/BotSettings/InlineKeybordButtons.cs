using Contracts;
using Models;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotSettings;

public class InlineKeyboardButtons
{
    private readonly string _apiPath;
    private readonly Order _order;
    private readonly List<ICategory> _categories;
    private readonly List<ISupply> _supplies;

    public InlineKeyboardButtons(string apiPath, List<ICategory> categories, List<ISupply> supplies, Order order)
    {
        _apiPath = apiPath;
        _categories = categories;
        _supplies = supplies;
        _order = order;
    }

    public InlineKeyboardMarkup GetCategoryButtons()
    {
        List<List<InlineKeyboardButton>> rowList = new();

        List<InlineKeyboardButton> categoryButtons = _categories
            .Select(c => InlineKeyboardButton.WithCallbackData(text: c.Name, callbackData: c.Id.ToString()))
            .ToList();


        List<InlineKeyboardButton> cancelButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "отмена заказа", callbackData: "cancel")
        };

        if (!_order.SuppliesId.Any())
        {
            rowList.Add(categoryButtons);
            rowList.Add(cancelButton);

            return new InlineKeyboardMarkup(rowList);
        }
        else
        {
            List<InlineKeyboardButton> acceptOrderButton = new()
            {
                InlineKeyboardButton.WithCallbackData(text: "Подтвердить заказ", callbackData: "accept")
            };

            var groupedSupplies = _order.SuppliesId.GroupBy(id => id);

            foreach (var supplyGroup in groupedSupplies)
            {
                string name = _supplies.FirstOrDefault(s => s.Id == supplyGroup.Key)?.Name ?? "";

                rowList.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: $"{name} - {supplyGroup.Count()}", callbackData: "notButton"),
                    InlineKeyboardButton.WithCallbackData(text: "+1", callbackData: $"add{supplyGroup.Key}"),
                    InlineKeyboardButton.WithCallbackData(text: "-1", callbackData: $"sub{supplyGroup.Key}"),

                });
            }
            rowList.Add(categoryButtons);
            rowList.Add(acceptOrderButton);
            rowList.Add(cancelButton);

            return new InlineKeyboardMarkup(rowList);
        }
    }

    public InlineKeyboardMarkup GetCategorySuppliesButtons(Guid categoryId)
    {
        List<List<InlineKeyboardButton>> rowList = new();


        List<InlineKeyboardButton> subcategoryButtons = _supplies
            .Where(s => s.CategoryId == categoryId)
            .Select(s => InlineKeyboardButton.WithCallbackData(text: s.Name, callbackData: s.Id.ToString()))
            .ToList();

        List<InlineKeyboardButton> backButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "back")
        };

        if (!_order.SuppliesId.Any())
        {
            rowList.Add(subcategoryButtons);
            rowList.Add(backButton);

            return new InlineKeyboardMarkup(rowList);
        }
        else
        {
            var groupedSupplies = _order.SuppliesId.GroupBy(id => id);

            foreach (var supplyGroup in groupedSupplies)
            {
                string name = _supplies.FirstOrDefault(s => s.Id == supplyGroup.Key)?.Name ?? "";

                rowList.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: $"{name} - {supplyGroup.Count()}", callbackData: "notButton"),
                    InlineKeyboardButton.WithCallbackData(text: "+1", callbackData: $"add{supplyGroup.Key}"),
                    InlineKeyboardButton.WithCallbackData(text: "-1", callbackData: $"sub{supplyGroup.Key}"),
                });
            }
            rowList.Add(subcategoryButtons);
            rowList.Add(backButton);

            return new InlineKeyboardMarkup(rowList);
        }
    }
}
