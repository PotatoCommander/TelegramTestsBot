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
        private static Menu currentMenu = new Menu("", "");
        public static void AddMenuToService(List<Menu> menus)
        {
            foreach (var menu in menus)
            {
                _allMenus.Add(menu);
            }
        }
        //creating buttons
        public static void ConnectTo(Menu origin, List<Menu> connectTo)
        {
            foreach (var menu in connectTo)
            {
                origin.AddButton(menu.name, menu.name, menu);
            }
        }
        public static void ButtonOnClick(CallbackQuery callback, ITelegramBotClient bot)
        {
            foreach (var menu in _allMenus)
            {
                if (menu.name == callback.Data)
                {
                    menu.DisplayMenu(callback.Message.Chat, bot);
                    currentMenu = menu;
                }
            }
            currentMenu.ClickOnButton(callback, bot);
        }
        public static void MenuDisplayByCommand(string name, Chat chat, ITelegramBotClient bot)
        {
            foreach (var menu in _allMenus)
            {
                if (menu.name == name)
                {
                    menu.DisplayMenu(chat, bot);
                    currentMenu = menu;
                }
            }
        }

    }
}
