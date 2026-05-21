using System.ComponentModel.DataAnnotations;

namespace ExpenseSystem.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "帳號必填")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "密碼必填")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "確認密碼必填")]
        [Compare("Password", ErrorMessage = "密碼和確認密碼不匹配")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "電子郵件必填")]
        [EmailAddress(ErrorMessage = "無效的電子郵件格式")]
        public string Email { get; set; } = string.Empty;


    }

}