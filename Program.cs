using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Abstractions;
using Tg.Buttons;
using Tg.Services;
using Tg.Menus;

namespace Tg
{
    class Program
    {
        private static ITelegramBotClient bot;

        static void Main(string[] args)
        {
            bot = new TelegramBotClient("1509037801:AAHSgqj_Xxt5Snksb3e25gy6TOBS4C_wxwM") { Timeout = TimeSpan.FromSeconds(10) };
            var info = bot.GetMeAsync().Result;
            var mainMenu = new Menu("main", "Главное меню :)");
            var test1Menu = new Menu("test1", "тестовое меню 1");
            var test2Menu = new Menu("test2", "тестовое меню 2");
            DisplayService.AddMenuToService(mainMenu);
            DisplayService.AddMenuToService(test1Menu);
            DisplayService.AddMenuToService(test2Menu);
            DisplayService.ConnectTo(mainMenu, new List<Menu> { test1Menu, test2Menu});


            bot.OnMessage += MessageGet;
            bot.OnCallbackQuery += Callback;
            bot.StartReceiving();
            Console.WriteLine($"Bot name: {info.FirstName}");
            Console.ReadKey();
        }

        private static async void MessageGet(object sender, MessageEventArgs e)
        {
            DisplayService.MenuDisplayByCommand("main", e.Message.Chat, bot);
          
        }
        private static async void Callback(object sender, CallbackQueryEventArgs ev)
        {
            DisplayService.ButtonOnClick(ev.CallbackQuery, bot);
        }
        private static  InlineKeyboardMarkup CreateMarkup(List<TelegramButton> buttons)
        {
            List<InlineKeyboardButton> mainMenuFormattedButtons = new List<InlineKeyboardButton>();
            foreach (TelegramButton button in buttons)
            {
                mainMenuFormattedButtons.Add(button.GetButton());
            }
            return new InlineKeyboardMarkup(mainMenuFormattedButtons);
        }
    }
}
