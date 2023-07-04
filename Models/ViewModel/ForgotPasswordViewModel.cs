using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "vui lòng nhập địa chỉ email")]
        public string Email { get; set; }
    }
}