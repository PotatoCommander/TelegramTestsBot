using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Tg.Buttons;
using Tg.Menus;

namespace Tg.Services
{
    public class Quiz
    {
        private ITelegramBotClient _botStarter;
        private Menu _menuFrom;
        private int _result;
        private DisplayBotService _starter;


        [JsonProperty] internal string QuizName { get; private set; }
        [JsonProperty] internal string QuizDefinition { get; private set; }
        [JsonProperty] internal List<Menu> questionMenus { get; private set; }

        public Quiz(string quizName, string quizDefinition, List<Menu> questions)
        {
            _result = 0;
            QuizName = quizName;
            QuizDefinition = quizDefinition;

            questionMenus = new List<Menu>
            {
                new Menu(quizDefinition,
                    buttons: new List<Button> {new Button("Начать тест!")})
            };
            questionMenus.AddRange(questions);
            var endMenu = new Menu("", buttons: new List<Button>
            {
                new Button("к списку!")
            });
            questionMenus.Add(endMenu);

            for (var i = 1; i < questionMenus.Count; i++)
                foreach (var answer in questionMenus[i - 1].Buttons)
                    answer.menuToDisplay = questionMenus[i];
        }

        public void Start(Menu menuFrom, ITelegramBotClient bot, DisplayBotService botService, Chat chat)
        {
            _result = 0;
            _botStarter = bot;
            _menuFrom = menuFrom;
            _starter = botService;
            _starter._currentMenu = questionMenus[0];
            _starter._currentMenu.DisplayMenu(chat, bot);
            bot.OnCallbackQuery -= _starter.ButtonOnClick;
            bot.OnCallbackQuery += QuizButtonClickHandler;

            questionMenus.Last().Buttons[0].menuToDisplay = _menuFrom;
        }

        public void QuizButtonClickHandler(object sender, CallbackQueryEventArgs ev)
        {
            if (_starter._currentMenu.Equals(questionMenus.Last()))
            {
                _botStarter.OnCallbackQuery -= QuizButtonClickHandler;
                _botStarter.OnCallbackQuery += _starter.ButtonOnClick;
            }

            foreach (var answer in _starter._currentMenu.Buttons)
            {
                if (ev.CallbackQuery.Data == answer.buttonCallbackData)
                {
                    _result += answer.AnswerWeight;
                    if (answer.menuToDisplay.Equals(questionMenus.Last()))
                        questionMenus.Last().Text = $"Конец теста:\n Ваш результат: {_result.ToString()}";
                    answer.Execute(ev.CallbackQuery.Message.Chat, _botStarter);
                    _starter._currentMenu = answer.menuToDisplay;
                    break;
                }
            }
        }
    }
}