using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Menus;

namespace Tg.Abstractions
{
    public abstract class TelegramButton
    {
        public abstract string text { get; set; }
        public abstract string buttonCallbackData { get; set; }
        public abstract Menu _menuToDisplay { get; set; }

        public abstract InlineKeyboardButton GetButton();
        public abstract void Execute(Chat chat, ITelegramBotClient bot);
    }
}
