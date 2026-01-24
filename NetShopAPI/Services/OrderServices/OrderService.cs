using NetShopAPI.Data;
using NetShopAPI.Models.OrderModel;
using NetShopAPI.DTOs.OrderDTO;
using NetShopAPI.DTOs.OrderDTO.Request;
using NetShopAPI.DTOs.OrderDTO.Response;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Models.CartModel;
using NetShopAPI.Models;
using NetShopAPI.Services.CurrentUserServices;
using NetShopAPI.Services.ClientServices;
using NetShopAPI.Common;
using NetShopAPI.Common.Order;
using static NetShopAPI.DTOs.OrderDTO.Response.OrderManagerDTO;

namespace NetShopAPI.Services.OrderServices
{
    public class OrderService : IOrderService
    {

        private readonly ShopDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public OrderService(ShopDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }



        public async Task<Result<OrderResponse>> CreateUserOrder(CreateOrderRequest req)
        {
            if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
                return Result<OrderResponse>.Unauthorized("UNAUTHORIZED", "Требуется авторизация!");
            
            if (req.ProductIds is null || req.ProductIds.Count == 0)
                return Result<OrderResponse>.BadRequest("NOT_SELECT_POSITION", "Не выбрано ни одной позиции.");

            var user = await GetUser(_currentUser.UserId.Value);

            if (!user.IsSucces || user.Data is null)
                return Result<OrderResponse>.NotFound("USER_NOT_FOUND", "Пользователь не найден.");

            var cart = await GetCartUser(user.Data);

            if (!cart.IsSucces || cart.Data is null || cart.Data.Items is null || !cart.Data.Items.Any())
                return Result<OrderResponse>.Conflict("USER_CART_IS_EMPTY", "Карзина пуста.");

            var selectedItems = cart.Data.Items.Where(i => req.ProductIds.Contains(i.ProductId)).ToList();

            if (!selectedItems.Any())
                return Result<OrderResponse>.BadRequest("NOT_SELECT_POSITION", "Не выбрано ни одной позиции.");

            var order = CreateOrder(user.Data);

            foreach (var item in selectedItems)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductNameSnapshot = item.Product.Name,
                    ProductDescriptionSnapshot = item.Product.Description,
                    ProductPriceSnapshot = item.Product.Price,
                    Quantity = item.Quantity
                });
            }

            order.TotalPrice = order.Items.Sum(i => i.ProductPriceSnapshot * i.Quantity);

            _db.Orders.Add(order);
            _db.CartItems.RemoveRange(selectedItems);
            await _db.SaveChangesAsync();

            return Result<OrderResponse>.Created(Map(order));
        }


        private async Task<Result<User>> GetUser(Guid userId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
                return Result<User>.NotFound("USER_NOT_FOUND", "Пользователь не найден.");

            return Result<User>.Ok(user);
        }


        private async Task<Result<Cart>> GetCartUser(User user)
        {
            var result = await _db.Carts.Include(c => c.Items).ThenInclude(i => i.Product).
                 FirstOrDefaultAsync(c => c.UserId == user.Id);

            return Result<Cart>.Ok(result);
        }


        private static Order CreateOrder(User user)
        {
            return new Order
            {
                UserId = user.Id,
                UserEmail = user.Email,
                Status = OrderStatus.Created,
                CreatedAtUtc = DateTime.UtcNow
            };
        }


        private static OrderResponse Map(Order order)
        {
            var result = new OrderResponse
            {
                Id = order.Id,
                UserEmail = order.UserEmail,
                Status = new OrderStatusDTO(order.Status.ToString(), GetDisplayNameOrderStatus(order.Status)),
                CreatedAt = order.CreatedAtUtc,
                TotalPrice = order.TotalPrice,

                Items = order.Items.Select(i => new OrderItemResponse
                {
                    ProductName = i.ProductNameSnapshot,
                    Price = i.ProductPriceSnapshot,
                    Quantity = i.Quantity
                }).ToList()
            };

            return result;
        }


        public async Task<Result<OrderResponse>> ChangeOrderStatusAsync(int orderId, OrderStatus NewStatus)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order is null)
                return Result<OrderResponse>.NotFound("ORDER_NOT_FOUND", "Заказ не найден.");

            var currentStat = order.Status;

            if (!OrderWorkflow.CanTransition(currentStat, NewStatus))
            {
                var allowed = string.Join(", ", OrderWorkflow.GetAllowedNext(currentStat));

                return Result<OrderResponse>.Conflict(
                    "INVALID_STATUS_TRANSITION",
                    $"Невозможно перевести статус заказа с {currentStat} в {NewStatus}! Допустимо: {allowed}.");
            }

            order.Status = NewStatus;
            
            await _db.SaveChangesAsync();

            return Result<OrderResponse>.Ok(Map(order));
        }


    }
}
