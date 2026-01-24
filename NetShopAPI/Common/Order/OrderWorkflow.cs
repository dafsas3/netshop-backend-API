namespace NetShopAPI.Common.Order
{
    public class OrderWorkflow
    {
        private static readonly Dictionary<OrderStatus, HashSet<OrderStatus>> Allowed = new()
        {
            [OrderStatus.Created] = new () { OrderStatus.Paid, OrderStatus.Cancelled },
            [OrderStatus.Paid] = new () { OrderStatus.Processing, OrderStatus.Cancelled },
            [OrderStatus.Processing] = new () { OrderStatus.Shipped, OrderStatus.Cancelled },
            [OrderStatus.Shipped] = new () { OrderStatus.Completed },
            [OrderStatus.Completed] = new(),
            [OrderStatus.Cancelled] = new()
        };

        public static bool CanTransition(OrderStatus from, OrderStatus to)
            => Allowed.TryGetValue(from, out var next) && next.Contains(to);

        public static IReadOnlyCollection<OrderStatus> GetAllowedNext(OrderStatus from)
            => Allowed.TryGetValue(from, out var next) ? next : Array.Empty<OrderStatus>();
    }
}