using System;
using System.Threading.Tasks;
using Example.Data.Customers;
using Example.Data.Orders;
using Example.Data.Tests.Integration.Customers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Example.Data.Tests.Integration.Orders
{
    public class OrderItemTests : IClassFixture<DbContextFixture>
    {
        private readonly ICustomerTestDataFactory _customerTestDataFactory;
        private readonly IOrderTestDataFactory _orderTestDataFactory;
        private readonly IOrderItemTestDataFactory _orderItemTestDataFactory;
        private readonly ExampleDbContext _setupDbContext;
        private readonly ExampleDbContext _verifyDbContext;

        public OrderItemTests(DbContextFixture dbContextFixture)
        {
            _customerTestDataFactory = dbContextFixture.ServiceProvider.GetService<ICustomerTestDataFactory>();
            _orderTestDataFactory = dbContextFixture.ServiceProvider.GetService<IOrderTestDataFactory>();
            _orderItemTestDataFactory = dbContextFixture.ServiceProvider.GetService<IOrderItemTestDataFactory>();
            _setupDbContext = dbContextFixture.SetupDbContext;
            _verifyDbContext = dbContextFixture.VerifyDbContext;
        }

        [Fact]
        public async Task CanReadWrite()
        {
            Customer setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            Order setupOrder = await _orderTestDataFactory.CreateAndAddToContextAsync(setupCustomer, _setupDbContext);
            OrderItem setupOrderItem = await _orderItemTestDataFactory.CreateAndAddToContextAsync(setupOrder, _setupDbContext);
            await _setupDbContext.SaveChangesAsync();

            try
            {
                OrderItem verifyOrderItem = await _verifyDbContext
                    .OrderItems
                    .Include(i => i.Order)
                    .SingleOrDefaultAsync(i => i.Id == setupOrderItem.Id);

                verifyOrderItem.Should().NotBeNull();
                verifyOrderItem.Order.Should().NotBeNull();
                verifyOrderItem.Order.Id.Should().Be(setupOrder.Id);
                verifyOrderItem.Modified.Should().BeBefore(DateTimeOffset.UtcNow);
                verifyOrderItem.ModifiedBy.Should().NotBeNull();
                verifyOrderItem.Name.Should().Be(setupOrderItem.Name);
                verifyOrderItem.Price.Should().Be(setupOrderItem.Price);
                verifyOrderItem.Quantity.Should().Be(setupOrderItem.Quantity);
            }
            finally
            {
                _setupDbContext.Remove(setupOrder);
                _setupDbContext.Remove(setupCustomer);
                _setupDbContext.RemoveRange(setupOrderItem);
                await _setupDbContext.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task AppliesQueryFilter()
        {
            Customer setupCustomer = await _customerTestDataFactory.CreateAndAddToContextAsync(_setupDbContext);
            Order setupOrder = await _orderTestDataFactory.CreateAndAddToContextAsync(setupCustomer, _setupDbContext);
            OrderItem setupOrderItem = await _orderItemTestDataFactory.CreateAndAddToContextAsync(setupOrder, _setupDbContext);
            setupOrderItem.IsDeleted = true;
            await _setupDbContext.SaveChangesAsync();

            try
            {
                OrderItem verifyOrderItem = await _verifyDbContext
                    .OrderItems
                    .SingleOrDefaultAsync(i => i.Id == setupOrderItem.Id);

                verifyOrderItem.Should().BeNull();
            }
            finally
            {
                _setupDbContext.Remove(setupOrder);
                _setupDbContext.Remove(setupCustomer);
                _setupDbContext.RemoveRange(setupOrderItem);
                await _setupDbContext.SaveChangesAsync();
            }
        }
    }
}