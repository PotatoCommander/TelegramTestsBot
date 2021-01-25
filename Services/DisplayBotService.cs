using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Tg.Menus;

namespace Tg.Services
{
    public class DisplayBotService
    {
        private readonly List<Menu> _allMenus = new List<Menu>();

        private readonly List<Quiz> _allTests = new List<Quiz>();
        private readonly ITelegramBotClient _bot;

        internal Menu _currentMenu;
        private bool _isStartedQuiz = false;
        private Menu _previewMenu; //need to

        private string _recognizedSpeech;

        public DisplayBotService(ITelegramBotClient bot)
        {
            _bot = bot;
            bot.OnCallbackQuery += ButtonOnClick;
            bot.OnMessage += MessageOnGet;
        }

        public void AddMenuToService(List<Menu> menus)
        {
            foreach (var menu in menus) _allMenus.Add(menu);
            _currentMenu = _allMenus[0];
        }

        public void AddTest(List<Quiz> tests)
        {
            foreach (var test in tests) _allTests.Add(test);
        }

        //creating buttons
        public void ConnectTo(Menu origin, List<Menu> connectTo)
        {
            foreach (var menu in connectTo)
            {
                origin.AddButton(menu.ShortDefinition, menu);
                menu.AddButton("Назад", origin);
            }
        }

        public void ButtonOnClick(object sender, CallbackQueryEventArgs ev)
        {
            var b = _currentMenu.Buttons.Find(button => button.buttonCallbackData == ev.CallbackQuery.Data);
            b?.Execute(ev.CallbackQuery.Message.Chat, _bot);
            if (b == null) return;
            _previewMenu = _currentMenu;
            _currentMenu = b.menuToDisplay;
        }

        public string GetQuizzes()
        {
            var quizNames = new string("");
            for (var i = 0; i < _allTests.Count; i++)
                quizNames = string.Concat(quizNames, $"{i + 1}: {_allTests[i].QuizName}\n");
            return quizNames;
        }

        public async Task<string> SpeechRecognition(string filename)
        {
            const string API_KEY = "AQVNy-VKWWRqCioH7Dp7kzUd1S1_Gq8tXGX6J0l2";

            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://stt.api.cloud.yandex.net/speech/v1/stt:recognize?topic=general"))
                {
                    request.Headers.TryAddWithoutValidation("Authorization", $"Api-Key {API_KEY}");

                    request.Content = new ByteArrayContent(await File.ReadAllBytesAsync(filename));

                    response = await httpClient.SendAsync(request);
                }
            }

            var contents = await response.Content.ReadAsStringAsync();
            var ycText = await Task.FromResult(contents.ToString());
            return ycText;
        }
        public async void AnalyzeVoice(MessageEventArgs ev)
        {
            var file = await _bot.GetFileAsync(ev.Message.Voice.FileId);
            var filePath = @"C:\TGAUDIO\" + ev.Message.Voice.FileId + ".wav";

            var fs = new FileStream(filePath, FileMode.Create);
            await _bot.DownloadFileAsync(file.FilePath, fs);
            fs.Close();
            await fs.DisposeAsync();
            var recognized = await SpeechRecognition(filePath);
            _ = _bot.SendTextMessageAsync(ev.Message.Chat.Id, recognized)
                .ConfigureAwait(false);
            Console.WriteLine($"{filePath}");
            Console.WriteLine($"Результат распознавания: {recognized}");
            //File.Delete(filePath);
        }

        public void MessageOnGet(object sender, MessageEventArgs ev)
        {
            if (ev.Message.Voice != null && _currentMenu.isVoiceAnalyzer)
            {
                _currentMenu.DisplayMenu(ev.Message.Chat, _bot);
                AnalyzeVoice(ev);
            }
            if (ev.Message.Text?.ToLowerInvariant() == "/start")
            {
                _allMenus[0].DisplayMenu(ev.Message.Chat, _bot);
                _currentMenu = _allMenus[0];
            }
            else
            {
                if (!_currentMenu.isQuizInitiatorPage) return;
                var n = 0;
                if (int.TryParse(ev.Message.Text, out n))
                {
                    n -= 1;
                    if (n < 0 || n >= _allTests.Count)
                        _bot.SendTextMessageAsync(ev.Message.Chat.Id,
                            "Неверно ввел сука, давай еще раз").ConfigureAwait(false);
                    else
                        _allTests[n].Start(_currentMenu, _bot, this, ev.Message.Chat);
                }
                else
                {
                    _bot.SendTextMessageAsync(ev.Message.Chat.Id, "Нужно писать число, шалунишка")
                        .ConfigureAwait(false);
                }
            }
        }
    }
}