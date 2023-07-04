using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Models.ViewModel
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập SDT")]
        public string PhoneNumber { get; set; }
        public IFormFile? avatar { get; set; }
        [Required(ErrorMessage = "vui lòng chọn quyền truy cập")]
        public string Role { get; set; }
    }
}
