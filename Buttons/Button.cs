using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Menus;
using Newtonsoft.Json;


namespace Tg.Buttons
{
    public class Button
    {
        public string text { get; set; }
        public string buttonCallbackData { get; set; }
        [JsonIgnore] public Menu _menuToDisplay { get; set; }
        public int? _answerWeight;

        public Button(string buttonText, string callbackData = "empty", Menu menu = null, int? weight = null)
        {
            text = buttonText;
            buttonCallbackData = callbackData;
            _menuToDisplay = menu;
            _answerWeight = null;
        }

        public async void Execute(Chat chat, ITelegramBotClient bot)
        {
            _menuToDisplay?.DisplayMenu(chat, bot);
        }

        public InlineKeyboardButton GetButton()
        {
            return InlineKeyboardButton.WithCallbackData(text, buttonCallbackData);
        }

    }
}
