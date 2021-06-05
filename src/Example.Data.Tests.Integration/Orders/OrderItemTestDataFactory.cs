using System;
using System.Threading.Tasks;
using Example.Data.Orders;

namespace Example.Data.Tests.Integration.Orders
{
    public interface IOrderItemTestDataFactory
    {
        OrderItem Create(Order order);
        Task<OrderItem> CreateAndAddToContextAsync(Order order, ExampleDbContext dbContext);
    }

    public class OrderItemTestDataFactory : IOrderItemTestDataFactory
    {
        public OrderItem Create(Order order)
        {
            return new OrderItem
            {
                Order = order,
                ModifiedBy = "integration test",
                Name = Guid.NewGuid().ToString(),
                Price = Convert.ToDecimal(Guid.NewGuid().GetHashCode()),
                Quantity = Guid.NewGuid().GetHashCode()
            };
        }

        public async Task<OrderItem> CreateAndAddToContextAsync(Order order, ExampleDbContext dbContext)
        {
            OrderItem item = Create(order);
            await dbContext.AddAsync(item);
            return item;
        }
    }
}