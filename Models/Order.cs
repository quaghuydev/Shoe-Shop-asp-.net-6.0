using ShoeShop.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        [ForeignKey("Address")]
        public Guid AddressId { get; set; }
        public Address Address { get; set; }
        public string Fullname { get; set; }
        public string? PhoneNumber { get; set; }
        public int TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public string PrintOrderDetail()
        {
            var result = "";
            foreach (OrderDetail detail in OrderDetails)
            {
                result += detail.ToString();
            }
            return result;
        }
    }
}
