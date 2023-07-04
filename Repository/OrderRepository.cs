using Microsoft.EntityFrameworkCore;
using ShoeShop.Dao;
using ShoeShop.Interfaces;
using ShoeShop.Models;
using ShoeShop.Models.Enums;
using ShoeShop.Models.ViewModel;

namespace ShoeShop.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUserRepository userRepository;
        private readonly ApplicationDbContext context;

        public OrderRepository(IHttpContextAccessor contextAccessor, IUserRepository userRepository, ApplicationDbContext context)
        {
            this.contextAccessor = contextAccessor;
            this.userRepository = userRepository;
            this.context = context;
        }
        public async Task<Order> CreateOrder(CheckoutViewModel model)
        {
            var currentUser = contextAccessor.HttpContext?.User.getUserId();
            var user = await userRepository.getUserById(currentUser);
            if (user != null)
            {
                var order = new Order
                {
                    UserId = user.Id,
                    Fullname = model.Fullname,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    PhoneNumber = user.PhoneNumber,
                    OrderStatus = OrderStatus.Improcess,
                    Address = new Address
                    {
                        NumberHouse = model.NumberHouse,
                        Street = model.Street,
                        Wards = model.Wards,
                        District = model.District,
                        Province = model.Province

                    },
                };
                context.Orders.Add(order);
                Save();
                return order;
            }
            return null;

        }

        public bool CreateOrderDetail(Order order, Cart cart)
        {
            foreach (var i in cart.Items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = i.Product.Id,
                    TotalPrice = i.SubTotal,
                    Price = i.Product.Price,
                    Quantity = i.Quantity,

                };
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
            }
            return Save();
        }



        public async Task<Order> GetById(Guid id)
        {
            return await context.Orders.Include(a => a.Address).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(Guid idOrder)
        {
            return await context.OrderDetails.Include(p => p.Product).Include(o => o.Order).Where(a => a.OrderId.Equals(idOrder)).ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await context.Orders.Include(a => a.Address).ToListAsync();
        }

        public bool Save()
        {
            var save = context.SaveChanges();
            return save > 0 ? true : false;
        }

        public bool UpdateOrder(Order order)
        {
            context.Orders.Update(order);
            return Save();

        }
        public bool DeleteOrder(Order order)
        {

            context.Orders.Remove(order);

            return Save();
        }

        public async Task<IEnumerable<Order>> GetOrderByUserId(string idUser)
        {
            return await context.Orders.Where(o => o.UserId.Equals(idUser)).ToListAsync();
        }

        public async Task<List<OrderDetail>> GetAllOrderById()
        {
            List<OrderDetail> list = new List<OrderDetail>();
            var orders = await GetOrders();
            foreach (var o in orders)
            {
                IEnumerable<OrderDetail> orderDetails = await GetOrderDetails(o.Id);
                list.AddRange(orderDetails);
            }
            return list;
        }
    }
}
