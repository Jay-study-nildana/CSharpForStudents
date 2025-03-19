using Microsoft.AspNetCore.Mvc;
using QuizOneMar192025.Models;
using QuizOneMar192025.ViewModels;

namespace QuizOneMar192025.Controllers
{
    public class QuizController : Controller
    {
        // This would typically come from a database
        private static List<Question> GetSampleQuestions()
        {
            return new List<Question>
            {
                new Question
                {
                    Id = 1,
                    Text = "What is the capital of France?",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, Text = "Berlin", IsCorrect = false },
                        new Answer { Id = 2, Text = "Madrid", IsCorrect = false },
                        new Answer { Id = 3, Text = "Paris", IsCorrect = true }
                    }
                },
                new Question
                {
                    Id = 2,
                    Text = "What is 2 + 2?",
                    Answers = new List<Answer>
                    {
                        new Answer { Id = 1, Text = "3", IsCorrect = false },
                        new Answer { Id = 2, Text = "4", IsCorrect = true },
                        new Answer { Id = 3, Text = "5", IsCorrect = false }
                    }
                }
            };
        }

        public IActionResult Index()
        {
            var model = new QuizViewModel
            {
                Questions = GetSampleQuestions()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SubmitQuiz(QuizViewModel model)
        {

            int correctAnswers = 0;
            var Questions = GetSampleQuestions();
            if (model.SelectedAnswers == null)
            {
                ViewBag.CorrectAnswers = 0;
                ViewBag.TotalQuestions = Questions.Count;
                return View("Results");
            }



            foreach (var question in Questions)
            {
                if (model.SelectedAnswers.TryGetValue(question.Id, out int selectedAnswerId))
                {
                    var correctAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect);
                    if (correctAnswer != null && correctAnswer.Id == selectedAnswerId)
                    {
                        correctAnswers++;
                    }
                }
            }

            ViewBag.CorrectAnswers = correctAnswers;
            //ViewBag.TotalQuestions = model.Questions.Count;
            //ViewBag.TotalQuestions = 0;
            ViewBag.TotalQuestions = Questions.Count;
            return View("Results");
        }
    }
}
