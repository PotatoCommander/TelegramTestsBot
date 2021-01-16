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
using Telegram.Bot.Types.InputFiles;
using Newtonsoft.Json;

namespace Tg.Services
{
    public class Quiz
    {
        public string _quizName;
        internal int? result { get; set; }
        internal int current = 0;

        private Menu _menuFrom;
        public ITelegramBotClient botStarter { get; private set; }

        private DisplayBotService starter = null;

        public string _quizDefinition;
        public List<Menu> questionMenus { get; set; }
        public Quiz(string quizName, string quizDefinition, List<Menu> questions)
        {
            _quizName = quizName;
            _quizDefinition = quizDefinition;
            questionMenus = new List<Menu>();
            questionMenus.Add(new Menu(quizDefinition,
                shortName: quizName,
                buttons: new List<Button>() { new Button("Начать тест!", null) }));
            questionMenus.AddRange(questions);
            Menu enMenu = new Menu("Ну типа конец", buttons: new List<Button>()
            {
                new Button("к списку!", menu:_menuFrom)
            });
            questionMenus.Add(enMenu);
            for (var i = 1; i < questionMenus.Count; i++)
            {
                foreach (var answer in questionMenus[i - 1].buttons)
                {
                    answer._menuToDisplay = questionMenus[i];
                    answer.buttonCallbackData = questionMenus[i].menuIdentifier;
                }
            }

            questionMenus[^1].buttons[0]._menuToDisplay = _menuFrom;
        }
        public void Start(Menu menuFrom, ITelegramBotClient bot, DisplayBotService botService, Chat chat)
        {
            botStarter = bot;
            _menuFrom = menuFrom;
            starter = botService;
            starter._currentMenu = questionMenus[0];
            starter._currentMenu.DisplayMenu(chat,bot);
            bot.OnCallbackQuery -= starter.ButtonOnClick;
            bot.OnCallbackQuery += QuizButtonClickHandler;

        }

        public void QuizButtonClickHandler(object sender, CallbackQueryEventArgs ev)
        {
            if (current == questionMenus.Count-1)
            {
                botStarter.OnCallbackQuery -= QuizButtonClickHandler;
                botStarter.OnCallbackQuery += starter.ButtonOnClick;
                var res = result.ToString();
                botStarter.SendTextMessageAsync(ev.CallbackQuery.Message.Chat.Id,
                    res).ConfigureAwait(false);
                starter._currentMenu = _menuFrom;
                _menuFrom.DisplayMenu(ev.CallbackQuery.Message.Chat,botStarter);
                return;
            }
            foreach (var answer in starter._currentMenu.buttons )
            {
                if (ev.CallbackQuery.Data == answer.buttonCallbackData)
                {
                    answer.Execute(ev.CallbackQuery.Message.Chat, botStarter);
                    starter._currentMenu = answer._menuToDisplay;
                    current++;
                    result += answer._answerWeight;
                    break;
                }
            }
        }
    }
}
