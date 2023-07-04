using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string First { get; set; }
        public string Second { get; set; }
        public string Third { get; set; }
    }
}
