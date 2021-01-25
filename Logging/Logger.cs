using System.Globalization;
using System.IO;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;

namespace Tg.Logging
{
    public class Logger
    {
        private readonly string _path;

        public Logger(string path)
        {
            _path = path;
        }

        public async void LoggerFileHandler(object sender, UpdateEventArgs ev)
        {
            var type = ev.Update.Type;
            string log;

            switch (type)
            {
                case UpdateType.Message:
                    log = $"    When: {ev.Update.Message.Date.ToString(CultureInfo.InvariantCulture)}\n " +
                          $"  From: {ev.Update.Message.From.Username} | " +
                          $"Name: {ev.Update.Message.From.FirstName} " +
                          $"{ev.Update.Message.From.LastName} | " +
                          $"LANG{ev.Update.Message.From.LanguageCode}\n" +
                          $"Message: {ev.Update.Message.Text}\n" +
                          $"ChatID: {ev.Update.Message.Chat.Id}";
                    break;
                case UpdateType.CallbackQuery:
                    log =
                        $"    When: {ev.Update.CallbackQuery.Message.Date.ToString(CultureInfo.InvariantCulture)}\n " +
                        $"  From: {ev.Update.CallbackQuery.From.Username} | " +
                        $"Name: {ev.Update.CallbackQuery.From.FirstName}  " +
                        $"{ev.Update.CallbackQuery.From.LastName} | " +
                        $"LANG {ev.Update.CallbackQuery.From.LanguageCode}\n" +
                        $"Callback data: {ev.Update.CallbackQuery.Data}\n" +
                        $"ChatID: {ev.Update.CallbackQuery.Message.Chat.Id}";
                    break;
                default:
                    log = $"unhandled update. {ev.Update.Id}";
                    break;
            }

            log = $"Update type: {type}\n" + log + "\n\n";
            await File.AppendAllTextAsync(_path, log);
        }
    }
}