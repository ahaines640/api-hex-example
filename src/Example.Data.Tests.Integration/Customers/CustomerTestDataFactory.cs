using System;
using System.Threading.Tasks;
using Example.Data.Customers;

namespace Example.Data.Tests.Integration.Customers
{
    public interface ICustomerTestDataFactory
    {
        Task<Customer> CreateAndAddToContextAsync(ExampleDbContext dbContext);
        Customer Create();
    }

    public class CustomerTestDataFactory : ICustomerTestDataFactory
    {
        public async Task<Customer> CreateAndAddToContextAsync(ExampleDbContext dbContext)
        {
            var customer = Create();
            await dbContext.AddAsync(customer);
            return customer;
        }

        public Customer Create()
        {
            return new()
            {
                ModifiedBy = "integration test",
                Name = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString().Substring(0, 25)
            };
        }
    }
}