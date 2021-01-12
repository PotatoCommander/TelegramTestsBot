using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Abstractions;
using Tg.Menus;


namespace Tg.Buttons
{
    public class Button : TelegramButton
    {
        public override string text { get; set; }
        public override string buttonCallbackData { get; set; }
        public override Menu _menuToDisplay { get; set; }

        public Button(string buttonText, string callbackData, Menu menu = null)
        {
            text = buttonText;
            buttonCallbackData = callbackData;
            _menuToDisplay = menu;
        }
        public override async void Execute(Chat chat, ITelegramBotClient bot)
        {
            if (_menuToDisplay!=null)
            {
                _menuToDisplay.DisplayMenu(chat, bot);
            }
        }

        public override InlineKeyboardButton GetButton()
        {
            return InlineKeyboardButton.WithCallbackData(text, buttonCallbackData);
        }

    }
}
