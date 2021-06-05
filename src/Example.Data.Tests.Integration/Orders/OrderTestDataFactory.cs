using System;
using System.Threading.Tasks;
using Example.Data.Customers;
using Example.Data.Orders;

namespace Example.Data.Tests.Integration.Orders
{
    public interface IOrderTestDataFactory
    {
        Task<Order> CreateAndAddToContextAsync(Customer customer, ExampleDbContext dbContext);
        Order Create(Customer customer);
    }

    public class OrderTestDataFactory : IOrderTestDataFactory
    {
        public async Task<Order> CreateAndAddToContextAsync(Customer customer, ExampleDbContext dbContext)
        {
            var order = Create(customer);
            await dbContext.AddAsync(order);
            return order;
        }

        public Order Create(Customer customer)
        {
            return new()
            {
                Customer = customer,
                ModifiedBy = "integration test",
                OrderNumber = Guid.NewGuid().ToString().Substring(0, 25)
            };
        }
    }
}