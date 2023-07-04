using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Dto
{
    public class RegisterDto
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
        public string Fullname { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập SDT")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "vui lòng chọn ảnh")]
        public IFormFile avatar { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
