using QuizOneMar192025.Models;

namespace QuizOneMar192025.ViewModels
{
    public class QuizViewModel
    {
        public List<Question> Questions { get; set; }
        public Dictionary<int, int> SelectedAnswers { get; set; }
    }

}
