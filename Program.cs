using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Abstractions;
using Tg.Buttons;
using Tg.Services;

namespace Tg
{
    class Program
    {
        private static ITelegramBotClient bot;

        static void Main(string[] args)
        {
            bot = new TelegramBotClient("1509037801:AAHSgqj_Xxt5Snksb3e25gy6TOBS4C_wxwM") { Timeout = TimeSpan.FromSeconds(10) };
            var info = bot.GetMeAsync().Result;
            DisplayService.CreateMenu("main", "ГЛАВНОЕ МЕНЮ");
            DisplayService.CreateMenu("test1", "PIPIPUPU WHATS UP BRO?");
            DisplayService.CreateMenu("test2", "Второе меню");


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
