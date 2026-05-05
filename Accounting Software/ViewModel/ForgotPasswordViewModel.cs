using System.ComponentModel.DataAnnotations;

namespace Accounting_Software.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = "";
    }
}
