using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Abstractions;
using Tg.Buttons;

namespace Tg.Menus
{
    public class Menu
    {
        private InlineKeyboardMarkup menu;
        private List<TelegramButton> _buttons;
        public string text { get; set; }
        public string name { get; set; }
        public Menu(string menuName, string menuText, List<TelegramButton> buttons)
        {
            name = menuName;
            text = menuText;
            _buttons = buttons;
        }
        public List<string> GetCallbacksOfMenu()
        {
            List<string> callbacks = new List<string>();
            foreach (TelegramButton button in _buttons)
            {
                callbacks.Add(button.buttonCallbackData);
            }
            return callbacks;
        }
        public async void DisplayMenu(Chat chat, ITelegramBotClient bot)
        {
            await bot.SendTextMessageAsync(chat , text,
                           replyMarkup: menu).ConfigureAwait(false);
        }
    }
}
