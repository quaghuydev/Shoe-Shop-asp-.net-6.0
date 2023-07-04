using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Dto
{
    public class LoginDto
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
