using System.ComponentModel.DataAnnotations;

namespace MyProject.Models
{
    public class PersonForm
    {
        [Required(ErrorMessage = "Пожалуйста, введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите email")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Range(14, 100, ErrorMessage = "Возраст должен быть от 14 до 100 лет")]
        [Display(Name = "Возраст")]
        public int Age { get; set; }
    }
}