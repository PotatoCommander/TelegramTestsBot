using System;
using System.Collections.Generic;
using System.Text;
using Tg.Buttons;
using Tg.Menus;
using Tg.Abstractions;
using System.Linq;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Tg.Services
{
    public static class DisplayService
    {
        //adding removing buttons/menus executing by callback data
        private static List<Menu> _allMenus = new List<Menu>();
        private static List<TelegramButton> _allButtons = new List<TelegramButton>();
        public static void CreateMenu(string name, string text, List<TelegramButton> buttons = null)
        {
            _allMenus.Add(new Menu(name, text, buttons));
            if (buttons != null)
            {
                _allButtons = (List<TelegramButton>)_allButtons.Union(buttons);
            }
        }
        public static void AddButtonTo(string menuName, string buttonText,string buttonCallback, Menu displayTo = null)
        {

        }
        public static void RemoveButtonFrom()
        { }
        public static void RemoveMenu()
        { }
        public static void ButtonExecute(CallbackQuery callback, ITelegramBotClient bot)
        { 
            foreach (var button in _allButtons)
            {
                if (button.buttonCallbackData == callback.Data)
                {
                    button.Execute(callback.Message.Chat, bot);
                }
            }
        }
        public static void MenuDisplayByCommand(string name, Chat chat, ITelegramBotClient bot)
        {
            foreach (var menu in _allMenus)
            {
                if (menu.name == name)
                {
                    menu.DisplayMenu(chat, bot);
                }
            }
        }

    }
}
