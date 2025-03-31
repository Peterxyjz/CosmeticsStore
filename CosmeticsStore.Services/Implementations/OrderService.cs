using CosmeticsStore.Repositories.Interfaces;
using CosmeticsStore.Repositories.Models;
using CosmeticsStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticsStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderDetailRepository orderDetailRepository,
            ICartRepository cartRepository,
            ICartItemRepository cartItemRepository,
            IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public Order GetOrderById(int id)
        {
            return _orderRepository.GetOrderWithDetails(id);
        }

        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            return _orderRepository.GetOrdersByUserId(userId);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orderRepository.GetAll();
        }

        //public int CreateOrder(int userId, string shippingAddress, string paymentMethod)
        //{
        //    var cart = _cartRepository.GetActiveCartByUserId(userId);

        //    if (cart == null || !cart.CartItems.Any())
        //    {
        //        throw new InvalidOperationException("Cart is empty");
        //    }

        //    // Check product availability and calculate total amount
        //    decimal totalAmount = 0;
        //    foreach (var item in cart.CartItems)
        //    {

        //        var product = _productRepository.GetById(item.ProductId);
        //        if (product == null || product.StockQuantity < item.Quantity)
        //        {
        //            throw new InvalidOperationException($"Product '{product?.ProductName}' is not available or has insufficient stock");
        //        }
        //        totalAmount += item.Price * item.Quantity;
        //    }

        //    // Create order
        //    var order = new Order
        //    {
        //        UserId = userId,
        //        OrderDate = DateTime.Now,
        //        TotalAmount = totalAmount,
        //        ShippingAddress = shippingAddress,
        //        Status = "Pending",
        //        PaymentMethod = paymentMethod,
        //        PaymentStatus = "Unpaid"
        //    };

        //    _orderRepository.Add(order);

        //    // Create order details and update product stock
        //    foreach (var item in cart.CartItems)
        //    {
        //        var orderDetail = new OrderDetail
        //        {
        //            OrderId = order.OrderId,
        //            ProductId = item.ProductId,
        //            Quantity = item.Quantity,
        //            Price = item.Price
        //        };
        //        _orderDetailRepository.Add(orderDetail);

        //        // Update product stock
        //        var product = _productRepository.GetById(item.ProductId);
        //        product.StockQuantity -= item.Quantity;
        //        _productRepository.Update(product);
        //    }

        //    // Deactivate the cart
        //    cart.Status = false;
        //    _cartRepository.Update(cart);

        //    return order.OrderId;
        //}

        public int CreateOrder(int userId, string shippingAddress, string paymentMethod)
        {
            var cart = _cartRepository.GetActiveCartByUserId(userId);

            if (cart == null || !cart.CartItems.Any())
            {
                throw new InvalidOperationException("Cart is empty");
            }
            // Check product availability and calculate total amount
            decimal totalAmount = 0;

            foreach (var item in cart.CartItems)
            {
                // Kiểm tra và ép kiểu từ int? sang int
                if (!item.ProductId.HasValue)
                {
                    throw new InvalidOperationException("Cart item has invalid product ID");
                }

                var product = _productRepository.GetById(item.ProductId.Value);
                if (product == null || product.StockQuantity < item.Quantity)
                {
                    throw new InvalidOperationException($"Product '{product?.ProductName}' is not available or has insufficient stock");
                }
                totalAmount += item.Price * item.Quantity;
            }
            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                ShippingAddress = shippingAddress,
                Status = "Processing",
                PaymentMethod = paymentMethod,
                PaymentStatus = "Paid"
            };
            _orderRepository.Add(order);
            // Create order details and update product stock
            foreach (var item in cart.CartItems)
            {
                if (!item.ProductId.HasValue)
                {
                    continue; // Bỏ qua item không có ProductId
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId.Value, // Sử dụng .Value để chuyển từ int? sang int
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _orderDetailRepository.Add(orderDetail);
                // Update product stock
                var product = _productRepository.GetById(item.ProductId.Value); // Sử dụng .Value ở đây nữa
                product.StockQuantity -= item.Quantity;
                _productRepository.Update(product);
            }
            // Deactivate the cart
            cart.Status = false;
            _cartRepository.Update(cart);
            return order.OrderId;
        }



        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _orderRepository.GetById(orderId);
            if (order != null)
            {
                order.Status = status;
                _orderRepository.Update(order);
            }
        }

        public void UpdatePaymentStatus(int orderId, string paymentStatus)
        {
            var order = _orderRepository.GetById(orderId);
            if (order != null)
            {
                order.PaymentStatus = paymentStatus;
                _orderRepository.Update(order);
            }
        }
    }
}
