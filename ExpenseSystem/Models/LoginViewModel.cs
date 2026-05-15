using System.ComponentModel.DataAnnotations;

namespace ExpenseSystem.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "帳號必填")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼必填")]
        public string Password { get; set; } = string.Empty;

    }

}