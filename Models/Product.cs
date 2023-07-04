using ShoeShop.Models.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public Gender Gender { get; set; }
        [ForeignKey("Image")]
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public int Quantity { get; set; }
        public int Sale { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
