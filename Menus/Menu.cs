using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Buttons;
using Newtonsoft.Json;

namespace Tg.Menus
{
    public class Menu
    {
        internal string shortDefinition { get; set; }
        internal string text { get; set; }
        internal string picUrl { get; set; }
        internal List<Button> buttons { get; set; }

        internal InlineKeyboardMarkup menuMarkup { get; set; }
        internal string menuIdentifier { get; set; }
        internal bool reactionOnCommand { get; set; }
        internal bool isQuizInitiatorPage { get; set; }

        public Menu(string menuText, string pictureUrl = null, string shortName = null, 
                    List<Button> buttons = null, bool quizInitPage = false)
        {
            shortDefinition = shortName;
            text = menuText;
            this.buttons = buttons;
            picUrl = pictureUrl;
            if (buttons == null) { this.buttons = new List<Button>(); }
            else { this.buttons = buttons; }
            isQuizInitiatorPage = quizInitPage;
            menuIdentifier = Guid.NewGuid().ToString();
        }
        public void AddButton(string buttonText, string buttonCallback, Menu displayTo = null, bool isQuestion = false, int? ansWeight = null)
        {
                buttons.Add(new Button(buttonText, buttonCallback, displayTo)); 
        }
        protected  InlineKeyboardMarkup CreateMarkup()
        {
            var markupButtons = new List<InlineKeyboardButton>();
            foreach (var button in buttons)
            {
                markupButtons.Add(button.GetButton());
            }
            return new InlineKeyboardMarkup(markupButtons);
        }
        public  void ClickOnButton(CallbackQuery callback, ITelegramBotClient bot)
        {
            foreach (var button in buttons)
            {
                if (callback.Data == button.buttonCallbackData) button.Execute(callback.Message.Chat, bot);
            }
        }
        public async  void DisplayMenu(Chat chat, ITelegramBotClient bot)
        {
            if (picUrl != null)
            {
                await bot.SendPhotoAsync(chat.Id, picUrl);
            }

            var reply = CreateMarkup();
            await bot.SendTextMessageAsync(chat , text,
                           replyMarkup: reply).ConfigureAwait(false);
        }
    }
}
