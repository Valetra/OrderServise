using Contracts;
using Models;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using ChatBot.Managers;

namespace BotSettings;

public class InlineKeyboardButtons
{
    private readonly Order _order;
    private readonly List<ICategory> _categories;
    private readonly List<ISupply> _supplies;

    private readonly List<InlineKeyboardButton> _backButton = new()
    {
        InlineKeyboardButton.WithCallbackData(text: "Назад ⬅️", callbackData: "back")
    };

    private readonly List<InlineKeyboardButton> _bucketButton = new()
    {
        InlineKeyboardButton.WithCallbackData(text: "корзина 🛒", callbackData: "notButton")
    };

    List<InlineKeyboardButton> _acceptOrderButton = new()
    {
        InlineKeyboardButton.WithCallbackData(text: "Подтвердить заказ ☑️", callbackData: "confirm")
    };

    public InlineKeyboardButtons(List<ICategory> categories, List<ISupply> supplies, Order order)
    {
        _categories = categories;
        _supplies = supplies;
        _order = order;
    }

    public InlineKeyboardMarkup GetConfirmOrderButtons()
    {
        List<List<InlineKeyboardButton>> rowList = new();

        rowList.Add(new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData(text: "Совершить заказ ✅", callbackData: "accept")
        });
        rowList.Add(_backButton);

        return new InlineKeyboardMarkup(rowList);
    }

    public static void InsertButtonsInRowList(List<List<InlineKeyboardButton>> rowList, List<InlineKeyboardButton> buttons)
    {
        foreach (var button in buttons)
        {
            List<InlineKeyboardButton> buttonInRow = new()
                {
                    button
                };
            rowList.Add(buttonInRow);
        }
    }

    public InlineKeyboardMarkup GetCategoryButtons()
    {
        List<List<InlineKeyboardButton>> rowList = new();

        List<InlineKeyboardButton> categoryButtons = _categories
            .Select(c => InlineKeyboardButton.WithCallbackData(text: c.Name, callbackData: c.Id.ToString()))
            .ToList();


        List<InlineKeyboardButton> cancelButton = new()
        {
            InlineKeyboardButton.WithCallbackData(text: "Отмена заказа 🗑️", callbackData: "cancel")
        };

        if (!_order.SuppliesId.Any())
        {
            InsertButtonsInRowList(rowList, categoryButtons);
            rowList.Add(cancelButton);

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
                    InlineKeyboardButton.WithCallbackData(text: $"{name}", callbackData: "notButton")
                });
                rowList.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: $"{supplyGroup.Count()} шт", callbackData: "notButton"),
                    InlineKeyboardButton.WithCallbackData(text: "+1", callbackData: $"increment:{supplyGroup.Key}"),
                    InlineKeyboardButton.WithCallbackData(text: "-1", callbackData: $"decrement:{supplyGroup.Key}"),
                });
            }
            rowList.Add(_bucketButton);
            InsertButtonsInRowList(rowList, categoryButtons);
            rowList.Add(_acceptOrderButton);
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


        if (!_order.SuppliesId.Any())
        {
            InsertButtonsInRowList(rowList, subcategoryButtons);
            rowList.Add(_backButton);

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
                    InlineKeyboardButton.WithCallbackData(text: $"{name}", callbackData: "notButton")
                });
                rowList.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: $"{supplyGroup.Count()} шт", callbackData: "notButton"),
                    InlineKeyboardButton.WithCallbackData(text: "+1", callbackData: $"increment:{supplyGroup.Key}"),
                    InlineKeyboardButton.WithCallbackData(text: "-1", callbackData: $"decrement:{supplyGroup.Key}"),
                });
            }
            rowList.Add(_bucketButton);
            InsertButtonsInRowList(rowList, subcategoryButtons);
            rowList.Add(_backButton);

            return new InlineKeyboardMarkup(rowList);
        }
    }
}
