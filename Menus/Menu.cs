using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Tg.Abstractions;
using Tg.Buttons;

namespace Tg.Menus
{
    public class Menu
    {
        private InlineKeyboardMarkup menu;
        private List<TelegramButton> _buttons;
        public string text { get; set; }
        public string name { get; set; }
        public string picUrl { get; set; }
        public Menu(string menuName, string menuText, string pictureUrl = null, List<TelegramButton> buttons = null)
        {
            name = menuName;
            text = menuText;
            _buttons = buttons;
            picUrl = pictureUrl;
            if (buttons == null) { _buttons = new List<TelegramButton>(); }
            else { _buttons = buttons; }

        }
        public List<string> GetCallbacksOfMenu()
        {
            List<string> callbacks = new List<string>();
            foreach (TelegramButton button in _buttons)
            {
                callbacks.Add(button.buttonCallbackData);
            }
            return callbacks;
        }
        public void AddButton(string buttonText, string buttonCallback, Menu displayTo)
        {
            _buttons.Add(new Button(buttonText, buttonCallback, displayTo));
        }
        private InlineKeyboardMarkup CreateMarkup()
        {
            List<InlineKeyboardButton> markupButtons = new List<InlineKeyboardButton>();
            foreach (var button in _buttons)
            {
                markupButtons.Add(button.GetButton());
            }
            return new InlineKeyboardMarkup(markupButtons);
        }
        public  void ClickOnButton(CallbackQuery callback, ITelegramBotClient bot)
        {
            foreach (var button in _buttons)
            {
                if (callback.Data == button.buttonCallbackData) button.Execute(callback.Message.Chat,bot);
            }
        }
        public async void DisplayMenu(Chat chat, ITelegramBotClient bot)
        {
            if (picUrl != null)
            {
                await bot.SendPhotoAsync(chat.Id, picUrl);
            }
            await bot.SendTextMessageAsync(chat , text,
                           replyMarkup: CreateMarkup()).ConfigureAwait(false);
        }
    }
}
