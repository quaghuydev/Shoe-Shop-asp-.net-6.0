namespace ShoeShop.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int SubTotal
        {
            get
            {
                return Product.Price * Quantity + (Product.Price * Quantity * (Product.Sale / 10));
            }

        }
    }
    public class Cart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public void AddCart(Product product, int quantity = 1)
        {
            var item = Items.FirstOrDefault(p => p.Product.Id == product.Id);
            if (item == null)
            {
                Items.Add(new CartItem { Product = product, Quantity = quantity });
            }
            else
            {
                item.Quantity += quantity;
            }

        }
        public void UpdateCart(Product product, int quantity)
        {
            var item = Items.FirstOrDefault(p => p.Product.Id == product.Id);
            if (item != null)
            {
                item.Quantity = quantity;
            }
        }
        public void DateleItem(Product product) => Items.RemoveAll(p => p.Product.Id == product.Id);
        public int TotalPrice() => Items.Sum(p => p.SubTotal);


    }

}
