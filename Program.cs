using System;
using System.Collections.Generic;
using Telegram.Bot;
using Tg.Logging;
using Tg.Menus;
using Tg.Services;

namespace Tg
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Telegram bot console";
            Console.ForegroundColor = ConsoleColor.Green;

            var jsonFolder = @"c:\jsones";
            string logFilePath = @"C:\jsones\log.txt";

            const string token = "1509037801:AAHSgqj_Xxt5Snksb3e25gy6TOBS4C_wxwM";
            ITelegramBotClient _bot = new TelegramBotClient(token) {Timeout = TimeSpan.FromSeconds(60)};

            var info = _bot.GetMeAsync().Result;

            var botService = new DisplayBotService(_bot);
            var p1 = new QuizParser(jsonFolder);
            botService.AddTest(p1.ParseJson());


            var mainMenu = new Menu("Вас приветствует шиза-бот!:)", shortName: "main");
            var testListMenu = new Menu($"Список тестов:\n{botService.GetQuizzes()}  \n Введи номер теста!",
                shortName: "Тесты",
                quizInitPage: true);
            var usefulOptionsMenu = new Menu("Список команд или кнопок хз короче ченить прикручу",
                shortName: "Другие опции");

            botService.AddMenuToService(new List<Menu> {mainMenu, testListMenu, usefulOptionsMenu});
            botService.ConnectTo(mainMenu, new List<Menu> {testListMenu, usefulOptionsMenu});

            var logger = new Logger(logFilePath);
            _bot.OnUpdate += logger.LoggerFileHandler;

            _bot.StartReceiving();
            Console.WriteLine($"Bot name: {info.FirstName}");
            Console.ReadKey();
        }
    }
}