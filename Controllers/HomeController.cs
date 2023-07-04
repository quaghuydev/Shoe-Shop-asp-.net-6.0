using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Infrastructure;
using ShoeShop.Interfaces;
using ShoeShop.Models;
using ShoeShop.Models.ViewModel;
using System.Diagnostics;

namespace ShoeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderRepository orderRepository;
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUserRepository userRepository;
        private readonly IContactRepository contactRepository;
        private readonly IProductRepository productRepository;

        public HomeController(ILogger<HomeController> logger, IOrderRepository orderRepository, IHttpContextAccessor contextAccessor, IUserRepository userRepository, IContactRepository contactRepository, IProductRepository productRepository)
        {
            _logger = logger;
            this.orderRepository = orderRepository;
            this.contextAccessor = contextAccessor;
            this.userRepository = userRepository;
            this.contactRepository = contactRepository;
            this.productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var hintProducts = await productRepository.GetProductsHint();
            var dealProducts = await productRepository.GetProductsDeal();
            ViewData["ActivePage"] = "Index";
            ViewBag.HintProducts = hintProducts;
            ViewBag.DealProducts = dealProducts;
            return View();
        }
        public IActionResult Success()
        {
            ViewData["ActivePage"] = "Index";
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Checkout()
        {
            CheckoutViewModel model = new CheckoutViewModel();
            Cart Cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            ViewBag.Cart = Cart;
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await orderRepository.CreateOrder(model);
                Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
                order.TotalPrice = cart.TotalPrice();
                if (order != null)
                {
                    orderRepository.CreateOrderDetail(order, cart);

                }
                return RedirectToAction("Success");
            }
            return View(model);
        }
        public async Task<IActionResult> PersonalDetail()
        {
            var currentUser = contextAccessor.HttpContext?.User.getUserId();
            var user = await userRepository.getUserById(currentUser);
            IEnumerable<Order> orders = await orderRepository.GetOrderByUserId(currentUser);
            List<OrderDetail> historyOrders = new List<OrderDetail>();
            foreach (var order in orders)
            {
                var id = order.Id;
                IEnumerable<OrderDetail> orderDetail = await orderRepository.GetOrderDetails(id);
                order.OrderDetails = (ICollection<OrderDetail>)orderDetail;
                historyOrders.AddRange(orderDetail);
            }
            ViewBag.User = user;
            ViewBag.DetailsOrder = historyOrders;
            return View(orders);
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Contact(ContactViewModel model)
        {
            var currentUser = contextAccessor.HttpContext?.User.getUserId();
            var user = await userRepository.getUserById(currentUser);
            var contact = new Contact
            {
                UserId = user.Id,
                Email = user.Email,
                Subject = model.Subject,
                Content = model.Content,
                Status = Models.Enums.ContactStatus.Improcess,
                CreateDate = DateTime.Now,
            };
            contactRepository.add(contact);
            return View();
        }
    }
}