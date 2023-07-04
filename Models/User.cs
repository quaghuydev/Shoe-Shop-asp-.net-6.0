using Microsoft.AspNetCore.Identity;
using ShoeShop.Models.Enums;

namespace ShoeShop.Models
{
    public class User : IdentityUser
    {
        public UserStatus UserStatus { get; set; }
        public string UserRoles { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? CreateDate { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
