using ShoeShop.Models.Enum;

namespace ShoeShop.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
        public Gender Gender { get; set; }
        public IFormFile Image1 { get; set; }
        public IFormFile Image2 { get; set; }
        public IFormFile Image3 { get; set; }
        public int Quantity { get; set; }
        public int Sale { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
