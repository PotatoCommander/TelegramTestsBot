using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Services;
using Tg.Menus;
using System.IO;
using Newtonsoft.Json;
using Tg.Buttons;

namespace Tg
{
    class Program
    {
        private static ITelegramBotClient bot;
        //public static StreamWriter sw1 = new StreamWriter(@"D:\json.json", true);
        static void Main(string[] args)
        {
            const string token = "1509037801:AAHSgqj_Xxt5Snksb3e25gy6TOBS4C_wxwM";
            bot = new TelegramBotClient(token) { Timeout = TimeSpan.FromSeconds(60) };
            var info = bot.GetMeAsync().Result;

            DisplayBotService botService = new DisplayBotService(bot);
            var mainMenu = new Menu("Вас приветствует шиза-бот!:)", shortName: "main");
            var testListMenu = new Menu($"Тут типа список: \n Введи номер теста!",shortName:"Тесты",
                quizInitPage:true);
            var usefulOptionsMenu= new Menu("Список команд или кнопок хз короче ченить прикручу",shortName:"Другие опции");
            botService.AddMenuToService(new List<Menu>(){mainMenu,testListMenu,usefulOptionsMenu});
            botService.ConnectTo(mainMenu,new List<Menu>(){testListMenu,usefulOptionsMenu});

            Quiz newQuiz = new Quiz("тестик на еблана","здесь ты узнаешь ебанько ты или нет",
                new List<Menu>()
                {
                    new Menu("тебя били по голове?", buttons: new List<Button>()
                    {
                        new Button("ДА", weight:10),
                        new Button("NO",weight:0)
                    }),
                    new Menu("сильно били?", buttons: new List<Button>()
                    {
                        new Button("ДА", weight:10),
                        new Button("NO",weight:0)
                    }),
                    new Menu("а в дурке лежал?", buttons: new List<Button>()
                    {
                        new Button("ДА", weight:10),
                        new Button("NO",weight:0)
                    })});

            botService.AddTest(newQuiz);





            bot.StartReceiving();
            Console.WriteLine($"Bot name: {info.FirstName}");
            Console.ReadKey();
        }
    }
}
