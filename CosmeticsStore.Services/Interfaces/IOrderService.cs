using CosmeticsStore.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Services.Interfaces
{
    public interface IOrderService
    {
        Order GetOrderById(int id);
        IEnumerable<Order> GetOrdersByUserId(int userId);
        IEnumerable<Order> GetAllOrders();
        int CreateOrder(int userId, string shippingAddress, string paymentMethod);
        void UpdateOrderStatus(int orderId, string status);
        void UpdatePaymentStatus(int orderId, string paymentStatus);
    }
}
