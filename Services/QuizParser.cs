using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tg.Buttons;
using Tg.Menus;

namespace Tg.Services
{
    public class Answer
    {
        public int AnswerWeight { get; set; }
        public string Text { get; set; }
    }

    public class Question
    {
        public string Text { get; set; }
        public string PicUrl { get; set; }
        public List<Answer> Buttons { get; set; }
        public List<Button> GetListOfButtons()
        { 
            var list = Buttons.Select(button => new Button(button.Text, weight: button.AnswerWeight)).ToList();
            return list;
        }
    }

    public class Root
    {
        public string QuizName { get; set; }
        public string QuizDefinition { get; set; }
        public List<Question> questionMenus { get; set; }

        public List<Menu> GetListOfMenus()
        {
            var list = new List<Menu>();
            foreach (var menu in questionMenus)
            {
                list.Add(new Menu(menu.Text, menu.PicUrl, buttons: menu.GetListOfButtons()));
            }

            return list;
        }
    }
    public class QuizParser
    {
        private string _folderPath;
        private string[] _jsonFiles;
        public QuizParser(string path)
        {
            _folderPath = path ?? Directory.GetCurrentDirectory();
            _jsonFiles = Directory.GetFiles(_folderPath, "*.json");
        }
        public List<Quiz> ParseJson()
        {
            var quizzes = new List<Quiz>();
            foreach (var fileName in _jsonFiles)
            {
                var deserialized = JsonConvert.DeserializeObject<Root>(File.ReadAllText(fileName));
                quizzes.Add(new Quiz(deserialized.QuizName, deserialized.QuizDefinition, deserialized.GetListOfMenus()));
            }
            return quizzes;
        }

        public void ToJson(List<Quiz> quizzes)
        {
            foreach (var quiz in quizzes)
            {
                var filename = @"C:\Jsones\" + quiz.QuizName.ToLower().Replace(" ", "_") + ".json";
                var serialized = JsonConvert.SerializeObject(quiz, settings: new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
                File.WriteAllTextAsync(filename, serialized);

            }
        }
    }
}
