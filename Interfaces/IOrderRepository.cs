using ShoeShop.Models;
using ShoeShop.Models.ViewModel;

namespace ShoeShop.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> GetById(Guid id);
        Task<IEnumerable<OrderDetail>> GetOrderDetails(Guid idOrder);
        Task<List<OrderDetail>> GetAllOrderById();
        Task<IEnumerable<Order>> GetOrderByUserId(string idUser);
        Task<Order> CreateOrder(CheckoutViewModel model);
        bool CreateOrderDetail(Order order, Cart cart);
        bool UpdateOrder(Order order);
        bool DeleteOrder(Order order);
        bool Save();

    }
}
