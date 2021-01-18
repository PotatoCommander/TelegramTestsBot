using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Buttons;
using Newtonsoft.Json;

namespace Tg.Menus
{
    public class Menu
    {
        internal string ShortDefinition { get; set; }
        [JsonProperty] internal string Text { get; set; }
        [JsonProperty] internal string PicUrl { get; set; }
        [JsonProperty] internal List<Button> Buttons { get; set; }
        internal bool isQuizInitiatorPage { get; private set; }

        public Menu(string menuText, string pictureUrl = null, string shortName = null, 
                    List<Button> buttons = null, bool quizInitPage = false)
        {
            ShortDefinition = shortName;
            Text = menuText;
            this.Buttons = buttons;
            PicUrl = pictureUrl;
            if (buttons == null) { this.Buttons = new List<Button>(); }
            else { this.Buttons = buttons; }
            isQuizInitiatorPage = quizInitPage;
        }
        public void AddButton(string buttonText, Menu displayTo = null, bool isQuestion = false, int? ansWeight = null)
        {
                Buttons.Add(new Button(buttonText, displayTo)); 
        }
        protected  InlineKeyboardMarkup CreateMarkup()
        {
            var markupButtons = new List<InlineKeyboardButton>();
            foreach (var button in Buttons)
            {
                markupButtons.Add(button.GetButton());
            }
            return new InlineKeyboardMarkup(markupButtons);
        }
        public  void ClickOnButton(CallbackQuery callback, ITelegramBotClient bot)
        {
            foreach (var button in Buttons)
            {
                if (callback.Data == button.buttonCallbackData) button.Execute(callback.Message.Chat, bot);
            }
        }
        public async  void DisplayMenu(Chat chat, ITelegramBotClient bot)
        {
            if (PicUrl != null)
            {
                await bot.SendPhotoAsync(chat.Id, PicUrl);
            }

            var reply = CreateMarkup();
            await bot.SendTextMessageAsync(chat , Text,
                           replyMarkup: reply).ConfigureAwait(false);
        }
    }
}
