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
using System.IO;
using Telegram.Bot.Types.InputFiles;

namespace Tg
{
    class Program
    {
        private static ITelegramBotClient bot;
        public static StreamWriter sw = new StreamWriter(@"D:\log.txt",true);
        static void Main(string[] args)
        {
            bot = new TelegramBotClient("1509037801:AAHSgqj_Xxt5Snksb3e25gy6TOBS4C_wxwM") { Timeout = TimeSpan.FromSeconds(10) };
            var info = bot.GetMeAsync().Result;
            string bigFloppa = new string("https://sun9-73.userapi.com/impf/fH3SCD495o1Akra4mx0ZSnzAhEEWjOZyvdGnGg/lBK_xz9N3oE.jpg?size=537x604&quality=96&proxy=1&sign=74fd7c3f969d88ca2afb64d9b70b9cd7&type=album");
            string soScarry = new string("https://sun9-3.userapi.com/impf/RjJhljRz8PtIzwTy8mWlT7hOltpITF_n1pwH0Q/jFSEeTiGCGM.jpg?size=1280x720&quality=96&proxy=1&sign=34b8a82e331dbe2dd2a3af258bd2f53e&type=album");
            var mainMenu = new Menu("main", "Вас приветствует шиза-бот!:)", pictureUrl: bigFloppa);
            var noMenu = new Menu("Нет", "ты обидел шлёппу, пидорас, тебе от этого не отмыться",pictureUrl:soScarry);
            var ukrMenu = new Menu("Украина нелегитимнa","ты прав бро");
            DisplayService.AddMenuToService(new List<Menu> { mainMenu, noMenu, ukrMenu });
            DisplayService.ConnectTo(mainMenu, new List<Menu> { noMenu, ukrMenu });



            bot.OnMessage += MessageGet;
            bot.OnCallbackQuery += Callback;
            bot.OnUpdate += Logger;
            bot.StartReceiving();
            Console.WriteLine($"Bot name: {info.FirstName}");
            Console.ReadKey();
            sw.Write("\t\t\t\tEnd of program...\n\n");
            sw.Close();
        }

        private static async void MessageGet(object sender, MessageEventArgs e)
        {
            DisplayService.MenuDisplayByCommand("main", e.Message.Chat, bot);

        }
        private static async void Callback(object sender, CallbackQueryEventArgs ev)
        {
            DisplayService.ButtonOnClick(ev.CallbackQuery, bot);
            //StreamWriter sw = new StreamWriter(@"D:\log.txt", true);
            string log = new string($"From {ev.CallbackQuery.From.Username} id: {ev.CallbackQuery.From.Id}\n" +
                              $" Date: {ev.CallbackQuery.Message.Date}  data: {ev.CallbackQuery.Data}\n");
            await sw.WriteAsync(log);
        }
        private static async void Logger(object sender, UpdateEventArgs e)
        {
                string log = new string($"TYPE OF UPDATE {e.Update.Type} \n");
                await sw.WriteAsync(log);
        }
        private static InlineKeyboardMarkup CreateMarkup(List<TelegramButton> buttons)
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
