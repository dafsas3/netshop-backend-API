using NetShopAPI.Common.Order;

namespace NetShopAPI.DTOs.OrderDTO.Response
{
    public class OrderManagerDTO
    {
        public record OrderStatusDTO(string Code, string Name);

        public static string GetDisplayNameOrderStatus(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Created => "Создан",
                OrderStatus.Paid => "Оплачен",
                OrderStatus.Processing => "В обработке",
                OrderStatus.Shipped => "Отправлен",
                OrderStatus.Completed => "Завершён",
                OrderStatus.Cancelled => "Отменён",
                _ => "Неизвестно"
            };
        }
    }
}
