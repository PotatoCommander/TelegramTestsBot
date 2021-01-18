using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Services;
using Tg.Menus;
using System.IO;
using  Newtonsoft.Json;
using Tg.Buttons;
using File = System.IO.File;

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

            var newQuiz1 = new Quiz("тестик на еблана", "здесь ты узнаешь ебанько ты или нет",
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
            //var newQuiz2 = new Quiz("Фанни инглиш", "Проверь свой уровень английского",
            //    new List<Menu>()
            //    {
            //        new Menu("I .... Spanish with my sister.", buttons: new List<Button>()
            //        {
            //            new Button("Study", weight:1),
            //            new Button("Studies",weight:0)
            //        }),
            //        new Menu("My sister .... a shower every morning.", buttons: new List<Button>()
            //        {
            //            new Button("Take", weight:0),
            //            new Button("Takes",weight:1)
            //        }), 
            //        new Menu("This house .... to my grandmother.", buttons: new List<Button>()
            //        {
            //            new Button("Belong", weight:0),
            //            new Button("Belongs",weight:1)
            //        }),
            //        new Menu("Ann .... apples.", buttons: new List<Button>()
            //        {
            //            new Button("Doesn't likes", weight:0),
            //            new Button("Don't likes",weight:0),
            //            new Button("Doesn't like",weight:1)
            //        }),
            //        new Menu("We .... to buy new furniture.", buttons: new List<Button>()
            //        {
            //            new Button("Don't want", weight:1),
            //            new Button("Don't wants",weight:0),
            //            new Button("Doesn't want", weight:0)
            //        }),
            //        new Menu("My friend and I .... Italian.", buttons: new List<Button>()
            //        {
            //            new Button("Does not speak", weight:0),
            //            new Button("Don't speak",weight:1),
            //            new Button("Do not speaks",weight:0)
            //        }),
            //        new Menu("(Jane / to cook) well?\n" +
            //                 "1.Does Jane cook well?\n" +
            //                 "2.Jane cook well?\n" +
            //                 "3.Jane does cook well?", buttons: new List<Button>()
            //        {
            //            new Button("1", weight:1),
            //            new Button("2",weight:0),
            //            new Button("3", weight:0)
            //        }),
            //        new Menu("(you / to do) exercises every morning?\n" +
            //                 "1.Do you exercises every morning?\n" +
            //                 "2.Do you do exercises every morning?\n" +
            //                 "3.Does you do exercises every morning?", buttons: new List<Button>()
            //        {
            //            new Button("1"),
            //            new Button("2",weight:1),
            //            new Button("3")
            //        }),
            //        new Menu("What (you / to have) for breakfast?\n" +
            //                 "1.What have you for breakfast?\n" +
            //                 "2.What do you has for breakfast?\n" +
            //                 "3.What do you have for breakfast?", buttons: new List<Button>()
            //        {
            //            new Button("1"),
            //            new Button("2"),
            //            new Button("3",weight:1)
            //        }),
            //        new Menu("It's a nice day today. You (not to need) your umbrella.\n" +
            //                 "1.It's a nice day today. You need your umbrella.\n" +
            //                 "2.It's a nice day today. You doesn't need your umbrella.\n" +
            //                 "3.It's a nice day today. You don't need your umbrella.", buttons: new List<Button>()
            //        {
            //            new Button("1"),
            //            new Button("2"),
            //            new Button("3",weight:1)
            //        })
            //    });

            //var json = JsonConvert.SerializeObject(newQuiz2,settings: new JsonSerializerSettings
            //{
            //    Formatting = Formatting.Indented
            //});
            //Console.WriteLine(json);

            //botService.AddTest(newQuiz1);
            //botService.AddTest(newQuiz2);

            QuizParser p1 = new QuizParser(@"c:\jsones");
            botService.AddTest(p1.ParseJson());


            var mainMenu = new Menu("Вас приветствует шиза-бот!:)", shortName: "main");
            var testListMenu = new Menu($"Список тестов:\n{botService.GetQuizzes()}  \n Введи номер теста!",shortName:"Тесты",
                quizInitPage:true);
            var usefulOptionsMenu= new Menu("Список команд или кнопок хз короче ченить прикручу",shortName:"Другие опции");

            botService.AddMenuToService(new List<Menu>(){mainMenu,testListMenu,usefulOptionsMenu});
            botService.ConnectTo(mainMenu,new List<Menu>(){testListMenu,usefulOptionsMenu});

            






            bot.StartReceiving();
            Console.WriteLine($"Bot name: {info.FirstName}");
            Console.ReadKey();
        }
    }
}
