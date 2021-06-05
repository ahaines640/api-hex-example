using System;
using System.Linq;
using System.Threading.Tasks;
using Example.Data.Customers;

namespace Example.Data.Tests.Customers
{
    public class CustomerTestDataFactory
    {
        private const string TestUser = nameof(CustomerTestDataFactory);

        public static async Task<Customer> CreateAndSaveAsync(ExampleDbContext dbContext)
        {
            var id = dbContext.Customers.Any()
                ? dbContext.Customers.Max(c => c.Id) + 1
                : 1;

            var customer = new Customer
            {
                Id = id,
                Modified = DateTimeOffset.UtcNow,
                ModifiedBy = TestUser,
                IsDeleted = false,
                Name = Guid.NewGuid().ToString(),
                Address = Guid.NewGuid().ToString(),
                City = Guid.NewGuid().ToString(),
                State = Guid.NewGuid().ToString(),
                PostalCode = Guid.NewGuid().ToString()
            };

            await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();

            return customer;
        }
    }
}