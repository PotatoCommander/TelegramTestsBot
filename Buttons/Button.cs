using System;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Menus;

namespace Tg.Buttons
{
    public class Button
    {
        [JsonProperty] internal int AnswerWeight;
        [JsonProperty] internal string Text { get; set; }

        internal string buttonCallbackData { get; }
        internal Menu menuToDisplay { get; set; }

        public Button(string buttonText, Menu menu = null, int weight = 0)
        {
            Text = buttonText;
            buttonCallbackData = Guid.NewGuid().ToString();
            menuToDisplay = menu;
            AnswerWeight = weight;
        }

        public async void Execute(Chat chat, ITelegramBotClient bot)
        {
            menuToDisplay?.DisplayMenu(chat, bot);
        }

        public InlineKeyboardButton GetButton()
        {
            return InlineKeyboardButton.WithCallbackData(Text, buttonCallbackData);
        }
    }
}