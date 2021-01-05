using System;
using  Telegram;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Tg
{
    class Program
    {
        private static ITelegramBotClient bot;
        static void Main(string[] args)
        {
            bot = new TelegramBotClient("1509037801:AAHSgqj_Xxt5Snksb3e25gy6TOBS4C_wxwM") {Timeout = TimeSpan.FromSeconds(10)};
            var info = bot.GetMeAsync().Result;

            bot.OnMessage += MessageGet;
            bot.StartReceiving();
            Console.WriteLine($"Bot name: {info.FirstName}");
            Console.ReadKey();
        }

        private static async void MessageGet(object sender, MessageEventArgs e)
        {
            await bot.SendTextMessageAsync(e.Message.Chat,"Hello world!").ConfigureAwait(false);
        }
    }
}
