using ShoeShop.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public ContactStatus Status { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User? User { get; set; }
        public string? Feedback { get; set; }
        public DateTime CreateDate { get; set; }


    }
}
