using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    public class OrderDetail
    {
        [Key] public Guid Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }
        public int TotalPrice { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string ToString()
        {
            return Product.Title;
        }

    }
}
