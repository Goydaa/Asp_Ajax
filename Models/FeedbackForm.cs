using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class FeedbackForm
    {
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите сообщение")]
        public string Message { get; set; }
    }
}