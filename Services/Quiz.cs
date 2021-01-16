using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Buttons;
using Tg.Services;
using Tg.Menus;
using System.IO;
using System.Linq;
using Telegram.Bot.Types.InputFiles;
using Newtonsoft.Json;

namespace Tg.Services
{
    public class Quiz
    {
        public string _quizName;
        internal int result { get; set; } = 0;
        internal int current = 0;

        private Menu _menuFrom;
        public ITelegramBotClient botStarter { get; private set; }

        private DisplayBotService starter = null;

        public string _quizDefinition;
        public List<Menu> questionMenus { get; set; }
        public Quiz(string quizName, string quizDefinition, List<Menu> questions)
        {
            result = 0;
            _quizName = quizName;
            _quizDefinition = quizDefinition;
            questionMenus = new List<Menu>();
            questionMenus.Add(new Menu(quizDefinition,
                shortName: quizName,
                buttons: new List<Button>() { new Button("Начать тест!", null) }));
            questionMenus.AddRange(questions);

            var endMenu = new Menu("", buttons: new List<Button>()
            {
                new Button("к списку!")
            });
            questionMenus.Add(endMenu);

            for (var i = 1; i < questionMenus.Count; i++)
            {
                foreach (var answer in questionMenus[i - 1].buttons)
                {
                    answer._menuToDisplay = questionMenus[i];
                    answer.buttonCallbackData = Guid.NewGuid().ToString();
                }
            }
        }
        public void Start(Menu menuFrom, ITelegramBotClient bot, DisplayBotService botService, Chat chat)
        {
            current = 0;
            result = 0;
            botStarter = bot;
            _menuFrom = menuFrom;
            starter = botService;
            starter._currentMenu = questionMenus[0];
            starter._currentMenu.DisplayMenu(chat,bot);
            bot.OnCallbackQuery -= starter.ButtonOnClick;
            bot.OnCallbackQuery += QuizButtonClickHandler;

            questionMenus.Last().buttons[0]._menuToDisplay = _menuFrom;


        }

        public void QuizButtonClickHandler(object sender, CallbackQueryEventArgs ev)
        {
            if (starter._currentMenu.Equals(questionMenus.Last())) 
            {
                botStarter.OnCallbackQuery -= QuizButtonClickHandler;
                botStarter.OnCallbackQuery += starter.ButtonOnClick;


                //var endMenu = new Menu("", buttons: new List<Button>()
                //{
                //    new Button("к списку!", _menuFrom.menuIdentifier,_menuFrom)
                //});
                //starter._currentMenu = endMenu;
                //endMenu.DisplayMenu(ev.CallbackQuery.Message.Chat, botStarter);
                //return;
            }
            foreach (var answer in starter._currentMenu.buttons )
            {
                if (ev.CallbackQuery.Data == answer.buttonCallbackData)
                {
                    result += answer._answerWeight;
                    if (answer._menuToDisplay.Equals(questionMenus.Last()))
                    {
                        questionMenus.Last().text = $"Конец теста:\n Ваш результат: {result.ToString()}";
                    }
                    answer.Execute(ev.CallbackQuery.Message.Chat, botStarter);
                    starter._currentMenu = answer._menuToDisplay;
                    current++;
                    break;
                }
            }
        }
    }
}
