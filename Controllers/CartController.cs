using Microsoft.AspNetCore.Mvc;
using ShoeShop.Infrastructure;
using ShoeShop.Interfaces;
using ShoeShop.Models;

namespace ShoeShop.Controllers
{
    public class CartController : Controller
    {
        public Cart? Cart { get; set; }
        private readonly IProductRepository repository;

        public CartController(IProductRepository repository)
        {
            this.repository = repository;
        }
        public IActionResult Index()
        {
            int cartItemCount = 0;
            foreach (var i in GetCart().Items)
            {
                cartItemCount += i.Quantity;
            }
            ViewBag.CartItemCount = cartItemCount;
            return View(GetCart());
        }
        private Cart GetCart()
        {
            Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return Cart;

        }
        private void CommitCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        public async Task<IActionResult> AddToCart(int id)
        {

            var product = await repository.getProductByIdAsyn(id);
            if (product != null)
            {
                Cart = GetCart();
                Cart.AddCart(product);
                CommitCart(Cart);

            }

            return RedirectToAction("Index", "Product");
        }
        public async Task<IActionResult> BuyNow(int id)
        {
            var product = await repository.getProductByIdAsyn(id);
            if (product != null)
            {
                Cart = GetCart();
                Cart.AddCart(product);
                CommitCart(Cart);

            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCart(List<CartItem> list)
        {
            foreach (var item in list)
            {
                var product = await repository.getProductByIdAsyn(item.Product.Id);
                if (product != null)
                {
                    Cart = GetCart();
                    Cart.UpdateCart(product, item.Quantity);
                    CommitCart(Cart);
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteItem(int id)
        {
            var product = await repository.getProductByIdAsyn(id);
            if (product != null)
            {
                Cart = GetCart();
                Cart.DateleItem(product);
                CommitCart(Cart);
            }
            return RedirectToAction("Index");

        }

    }
}
