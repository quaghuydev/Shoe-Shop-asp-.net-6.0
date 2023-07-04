using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Dto
{
    public class UserDto
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "xác nhận mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập SDT")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "vui lòng chọn ảnh")]
        public IFormFile? avatar { get; set; }
        [Required(ErrorMessage = "vui lòng chọn quyền truy cập")]
        public string Role { get; set; }
    }
}
