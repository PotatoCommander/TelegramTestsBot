using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Tg.Menus;

namespace Tg.Services
{
    public class DisplayBotService
    {
        private ITelegramBotClient _bot;

        List<Menu> _allMenus = new List<Menu>();

        List<Quiz> _allTests = new List<Quiz>();

        internal Menu _currentMenu;
        private Menu _previewMenu = null; //need to
        private bool _isStartedQuiz = false;

        public DisplayBotService(ITelegramBotClient bot)
        {
            _bot = bot;
            bot.OnCallbackQuery += ButtonOnClick;
            bot.OnMessage += MessageOnGet;
        }

        public void AddMenuToService(List<Menu> menus)
        {
            foreach (var menu in menus)
            {
                _allMenus.Add(menu);
            }
            _currentMenu = _allMenus[0];
        }
        public void AddTest(Quiz tests) => _allTests.Add(tests);
        //creating buttons
        public void ConnectTo(Menu origin, List<Menu> connectTo)
        {
            foreach (var menu in connectTo)
            {
                origin.AddButton(menu.shortDefinition, menu.menuIdentifier, menu);
                menu.AddButton("Назад", origin.menuIdentifier, origin);
            }
        }

        public void ButtonOnClick(object sender, CallbackQueryEventArgs ev)
        {
            var b = _currentMenu.buttons.Find(button => button.buttonCallbackData == ev.CallbackQuery.Data);
            b?.Execute(ev.CallbackQuery.Message.Chat, _bot);
            if (b != null)
            {
                _previewMenu = _currentMenu;
                _currentMenu = b._menuToDisplay;
            }
        }
        public string GetQuizzes()
        {
            var quizNames = new string("");
            for (var i = 0; i < _allTests.Count; i++)
            {
                quizNames = string.Concat(quizNames, $"{i + 1}: {_allTests[i]._quizName}\n");
            }
            return quizNames;
        }

        public void MessageOnGet(object sender, MessageEventArgs ev)
        {
            if (ev.Message.Text.ToLowerInvariant() == "start")
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
                    if (n < 0 || n >= _allTests.Count)
                        _bot.SendTextMessageAsync(ev.Message.Chat.Id,
                            "Неверно ввел сука, давай еще раз").ConfigureAwait(false);
                    else
                    { _allTests[n].Start(_currentMenu, _bot, this, ev.Message.Chat); }
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
