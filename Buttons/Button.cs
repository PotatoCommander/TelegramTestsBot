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
        public override Menu _menuToDisplay { get; set; } = null;

        public Button(string text, string callbackData, Menu menu = null)
        {
            text = text;
            buttonCallbackData = callbackData;
            _menuToDisplay = menu;
        }
        public override async void Execute(Chat chat, ITelegramBotClient bot)
        {
            //TODO:
            //display menu
            if (_menuToDisplay!=null)
            {
                _menuToDisplay.DisplayMenu(chat, bot);
            }
        }

        public override InlineKeyboardButton GetButton()
        {
            throw new NotImplementedException();
        }
 
    }
}
