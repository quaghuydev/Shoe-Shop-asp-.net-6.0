
using System.ComponentModel.DataAnnotations;

namespace ShoeShop.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public string NumberHouse { get; set; }
        public string Street { get; set; }
        public string Wards { get; set; }//xã
        public string District { get; set; }//huyện
        public string Province { get; set; }//tỉnh
        public string ToString()
        {
            return NumberHouse + ", " + Street + ", " + Wards + ", " + District + ", " + Province;
        }
    }
}