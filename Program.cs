using System;
using System.Collections.Generic;
using Telegram.Bot;
using Tg.Logging;
using Tg.Menus;
using Tg.Services;

namespace Tg
{
    class Program
    {
        private static ITelegramBotClient bot;

        //public static StreamWriter sw1 = new StreamWriter(@"D:\json.json", true);
        static void Main(string[] args)
        {
            Console.Title = "Telegram bot console";
            Console.ForegroundColor = ConsoleColor.Green;

            const string token = "1509037801:AAHSgqj_Xxt5Snksb3e25gy6TOBS4C_wxwM";
            bot = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(60) };
            var info = bot.GetMeAsync().Result;

            var botService = new DisplayBotService(bot);
            QuizParser p1 = new QuizParser(@"c:\jsones");
            botService.AddTest(p1.ParseJson());


            var mainMenu = new Menu("Вас приветствует шиза-бот!:)", shortName: "main");
            var testListMenu = new Menu($"Список тестов:\n{botService.GetQuizzes()}  \n Введи номер теста!", shortName: "Тесты",
                quizInitPage: true);
            var usefulOptionsMenu = new Menu("Список команд или кнопок хз короче ченить прикручу", shortName: "Другие опции");

            botService.AddMenuToService(new List<Menu>() { mainMenu, testListMenu, usefulOptionsMenu });
            botService.ConnectTo(mainMenu, new List<Menu>() { testListMenu, usefulOptionsMenu });

            var logger = new Logger(@"C:\jsones\log.txt");
            bot.OnUpdate += logger.LoggerFileHandler;

            bot.StartReceiving();
            Console.WriteLine($"Bot name: {info.FirstName}");
            Console.ReadKey();
        }
    }
}
