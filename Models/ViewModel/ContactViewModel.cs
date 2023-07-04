using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Models.ViewModel
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập chủ đề")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string Content { get; set; }
    }
}
