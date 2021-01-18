using System;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Menus;
using Newtonsoft.Json;


namespace Tg.Buttons
{
    public class Button
    {
        [JsonProperty] internal int AnswerWeight;
        [JsonProperty] internal string Text { get; set; }


        internal string buttonCallbackData { get; private set; }
        internal Menu _menuToDisplay { get; set; }

        public Button(string buttonText, Menu menu = null, int weight = 0)
        {
            Text = buttonText;
            buttonCallbackData = Guid.NewGuid().ToString(); 
            _menuToDisplay = menu;
            AnswerWeight = weight;
        }

        public async void Execute(Chat chat, ITelegramBotClient bot)
        {
            _menuToDisplay?.DisplayMenu(chat, bot);
        }

        public InlineKeyboardButton GetButton()
        {
            return InlineKeyboardButton.WithCallbackData(Text, buttonCallbackData);
        }

    }
}
