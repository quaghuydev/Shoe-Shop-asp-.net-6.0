using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Interfaces;
using ShoeShop.Models;
using ShoeShop.Models.Enums;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("admin"))]
    public class OrderController : Controller
    {
        private readonly IOrderRepository orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Order> orders = await orderRepository.GetOrders();
            return View(orders);
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var order = await orderRepository.GetById(id);
            IEnumerable<OrderDetail> details = await orderRepository.GetOrderDetails(order.Id);
            ViewBag.Order = order;
            return View(details);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Guid id)
        {
            var existingOrder = await orderRepository.GetById(id);
            if (existingOrder != null)
            {


                if (existingOrder.OrderStatus == OrderStatus.Processing)
                {
                    existingOrder.OrderStatus = OrderStatus.Improcess;
                }
                else
                {
                    existingOrder.OrderStatus = OrderStatus.Processing;

                }
                // Cập nhật các thuộc tính khác
                existingOrder.UpdateDate = DateTime.Now;
                orderRepository.UpdateOrder(existingOrder);

                return Ok();
            }
            return BadRequest();
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var o = await orderRepository.GetById(id);
            if (o != null)
            {
                orderRepository.DeleteOrder(o);
                return Ok();
            }
            return BadRequest();
        }
    }
}
